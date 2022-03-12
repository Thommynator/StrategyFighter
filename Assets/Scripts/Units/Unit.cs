using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using MoreMountains.Feedbacks;
public class Unit : MonoBehaviour
{
    public UnitStats stats;
    public GameObject attackIcon;
    [HideInInspector] public bool isSelected;
    [HideInInspector] public bool hasMoved;
    [HideInInspector] public int currentRemainingAttacks;
    protected int currentHealth;
    private List<Unit> enemiesInRange = new List<Unit>();
    private StatsView statsView;
    private Camera mainCamera;
    private GameObject sleepFxObject;
    [SerializeField] private ParticleSystem sleepFx;
    [SerializeField] private MMFeedbacks placementFeedbacks;

    [Header("Sounds")]
    [SerializeField] private List<AudioClip> dieSounds;
    [SerializeField] private List<AudioClip> attackSounds;
    [SerializeField] private List<AudioClip> hitSounds;
    [SerializeField] private AudioClip selectSound;

    protected virtual void Start()
    {
        mainCamera = Camera.main;
        attackIcon.SetActive(false);
        currentHealth = stats.health;
        statsView = FindObjectOfType<StatsView>();
    }

    protected void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            statsView.ShowStatsOf(this);
        }
    }
    protected void OnMouseExit()
    {
        statsView.HideStats();
    }

    protected void OnMouseDown()
    {
        GameMaster.current.ResetPreviewUnit();
        HideAllAttackIcons();

        // deselect current unit
        if (isSelected)
        {
            isSelected = false;
            GameMaster.current.selectedUnit = null;
            GameMaster.current.ResetTiles();
            return;
        }

        if (GameMaster.current.IsCurrentPlayerByTag(tag))
        {
            // deselect previously selected unit
            if (GameMaster.current.selectedUnit != null)
            {
                GameMaster.current.selectedUnit.isSelected = false;
            }

            isSelected = true;
            GameMaster.current.selectedUnit = this;
            PlaySelectSound();
            GameMaster.current.ResetTiles();

            GetEnemiesInRange();
            HighlightWalkableTiles();
            HighlightAttackableTiles();
        }

        Collider2D collider = Physics2D.OverlapCircle(mainCamera.ScreenToWorldPoint(Input.mousePosition), 0.2f);
        Unit unit = collider.GetComponent<Unit>();
        if (unit != null && GameMaster.current.selectedUnit != null)
        {
            Unit selectedUnit = GameMaster.current.selectedUnit;
            if (selectedUnit.HasRemainingAttacks() && selectedUnit.enemiesInRange.Contains(unit))
            {
                selectedUnit.Attack(unit);
            }
        }
    }

    protected void HighlightWalkableTiles()
    {
        if (hasMoved)
        {
            return;
        }

        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (tile.IsClear() && transform.position.ManhattenDistanceTo(tile.transform.position) <= stats.movementRange)
            {
                tile.ShowReachableHighlight();
            }
        }
    }

    protected void HighlightAttackableTiles()
    {
        if (!HasRemainingAttacks())
        {
            return;
        }

        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            var distaceToTile = transform.position.ManhattenDistanceTo(tile.transform.position);
            if (distaceToTile > 0 && distaceToTile <= stats.attackRange)
            {
                tile.ShowAttackBorderHighlight();
            }
        }

    }

    protected void GetEnemiesInRange()
    {
        enemiesInRange.Clear();
        if (!HasRemainingAttacks())
        {
            return;
        }

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (transform.position.ManhattenDistanceTo(unit.transform.position) <= stats.attackRange)
            {
                // only find enemies
                if (!GameMaster.current.IsCurrentPlayerByTag(unit.tag))
                {
                    enemiesInRange.Add(unit);
                    unit.attackIcon.SetActive(true);
                }
            }
        }
    }

    protected void HideAllAttackIcons()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.attackIcon.SetActive(false);
        }
    }

    public void MoveTo(Vector2 position)
    {
        GameMaster.current.ResetTiles();
        transform
            .DOMoveX(position.x, stats.movementSpeed)
            .SetSpeedBased(true)
            .OnComplete(() =>
                transform
                .DOMoveY(position.y, stats.movementSpeed)
                .SetSpeedBased(true)
                .OnComplete(() =>
                {
                    HideAllAttackIcons();
                    GetEnemiesInRange();
                    hasMoved = true;

                    if (!CanMove() && !CanAttack())
                    {
                        Sleep();
                    }
                })
            );
    }

    protected bool HasRemainingAttacks()
    {
        return currentRemainingAttacks > 0;
    }

    protected virtual void Attack(Unit enemy)
    {
        if (!HasRemainingAttacks())
        {
            return;
        }

        currentRemainingAttacks -= 1;
        int dealingDamage = stats.attackDamage - enemy.stats.armor;
        int receivingDamage = enemy.stats.defenseDamage - stats.armor;

        DealDamage(enemy, dealingDamage);

        // only takes damage if this unit is inside of the attack range of the enemy unit
        if (transform.position.ManhattenDistanceTo(enemy.transform.position) <= enemy.stats.attackRange)
        {
            TakeDamage(receivingDamage);
        }

        HighlightWalkableTiles(); // check again, because maybe a previously blocking enemy might be dead now

        if (!CanMove() && !CanAttack())
        {
            Sleep();
        }
    }

    protected virtual void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }
        DamageIcon damageIcon = Instantiate(stats.damageIconPrefab, transform.position, Quaternion.identity);
        damageIcon.ChooseIconBasedOnDamage(damage);
        currentHealth -= damage;
        CameraShaker.current.Shake(CameraShaker.ShakeStrength.WEAK);
        CheckForDeath();
        SoundManager.instance.PlayRandomAudio(hitSounds);
    }

    protected void DealDamage(Unit enemy, int damage)
    {
        if (damage <= 0)
        {
            return;
        }
        SoundManager.instance.PlayRandomAudio(attackSounds);
        DamageIcon damageIcon = Instantiate(stats.damageIconPrefab, enemy.transform.position, Quaternion.identity);
        damageIcon.ChooseIconBasedOnDamage(damage);
        enemy.TakeDamage(damage);
    }

    protected void CheckForDeath()
    {
        if (currentHealth > 0)
        {
            return;
        }
        statsView.HideStats();
        Instantiate(stats.killParticleFx, transform.position, Quaternion.identity);
        GameMaster gm = GameMaster.current;
        SoundManager.instance.PlayRandomAudio(dieSounds);
        // self-killed, i.e. died in own turn
        if (gm.IsCurrentPlayerByTag(this.tag))
        {
            GameMaster.current.ResetTiles();
        }
        Destroy(this.gameObject);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void PlaySelectSound()
    {
        SoundManager.instance.PlayAudio(selectSound);
    }

    public void PlaceUnit()
    {
        placementFeedbacks.PlayFeedbacks();
        hasMoved = true;
        currentRemainingAttacks = 0;
        Sleep();
    }

    public void Sleep()
    {
        print("Sleep");
        print(sleepFx);
        sleepFx?.Play();
        SpriteRenderer[] spriteRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in spriteRenderers)
        {
            Color c = renderer.color;
            renderer.color = new Color(c.r, c.g, c.b, 0.7f);
        }
    }

    public void WakeUp()
    {
        sleepFx?.Stop();
        SpriteRenderer[] spriteRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in spriteRenderers)
        {
            Color c = renderer.color;
            renderer.color = new Color(c.r, c.g, c.b, 1.0f);
        }
    }

    private bool CanMove()
    {
        return !hasMoved;
    }

    private bool CanAttack()
    {
        return HasRemainingAttacks() && IsAnyEnemyInRange();
    }

    private bool IsAnyEnemyInRange()
    {
        IEnumerable<Unit> enemies = FindObjectsOfType<Unit>().Where(unit => !GameMaster.current.IsCurrentPlayerByTag(unit.tag));
        foreach (var enemy in enemies)
        {
            if (transform.position.ManhattenDistanceTo(enemy.transform.position) <= stats.attackRange)
            {
                return true;
            }
        }
        return false;
    }
}

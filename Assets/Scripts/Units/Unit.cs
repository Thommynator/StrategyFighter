using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Unit : ShopableItem
{
    public UnitStats stats;
    public GameObject attackIcon;
    [HideInInspector] public bool isSelected;
    [HideInInspector] public bool hasMoved;
    [HideInInspector] public int currentRemainingAttacks;
    protected int currentHealth;
    private List<Unit> enemiesInRange = new List<Unit>();
    private StatsView statsView;

    protected virtual void Start()
    {
        attackIcon.SetActive(false);
        currentHealth = stats.health;
        statsView = FindObjectOfType<StatsView>();
        currentRemainingAttacks = stats.numberOfAttacks;
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
            GameMaster.current.ResetTiles();

            GetEnemiesInRange();
            GetWalkableTiles();
        }

        Collider2D collider = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.2f);
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

    protected void GetWalkableTiles()
    {
        if (hasMoved)
        {
            return;
        }

        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (tile.IsClear() && transform.position.ManhattenDistanceTo(tile.transform.position) <= stats.movementRange)
            {
                tile.HighlightReachable();
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
                })
            );
        hasMoved = true;

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

        GetWalkableTiles(); // check again, because maybe a previously blocking enemy might be dead now
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
    }

    protected void DealDamage(Unit enemy, int damage)
    {
        if (damage <= 0)
        {
            return;
        }

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
}

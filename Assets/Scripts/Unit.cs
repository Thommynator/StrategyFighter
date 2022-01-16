using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Unit : MonoBehaviour
{
    public Player player;
    private Animator cameraAnimator;
    public GameObject killParticleFx;

    [Header("Movement")]
    public bool isSelected;
    public int movementRange;
    public float movementSpeed;
    public bool hasMoved;

    [Header("Combat")]
    public int attackRange;
    public bool hasAttacked;
    public List<Unit> enemiesInRange = new List<Unit>();
    public GameObject attackIcon;
    public DamageIcon damageIconPrefab;

    [Header("Stats")]
    public int health;
    public int armor;
    public int attackDamage;
    public int defenseDamage;

    [Header("King")]
    public bool isKing;
    public TextMeshProUGUI kingHealthText;

    void Start()
    {
        cameraAnimator = Camera.main.GetComponent<Animator>();
        attackIcon.SetActive(false);
        UpdateKingHealthText();
    }


    private void OnMouseDown()
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

        // it's not your turn
        if (player == GameMaster.current.GetCurrentPlayer())
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
            if (!selectedUnit.hasAttacked && selectedUnit.enemiesInRange.Contains(unit))
            {
                selectedUnit.Attack(unit);
            }
        }
    }

    private void GetWalkableTiles()
    {
        if (hasMoved)
        {
            return;
        }

        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (tile.IsClear() && transform.position.ManhattenDistanceTo(tile.transform.position) <= movementRange)
            {
                tile.Highlight();
            }
        }
    }

    private void GetEnemiesInRange()
    {
        enemiesInRange.Clear();
        if (hasAttacked)
        {
            return;
        }

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (transform.position.ManhattenDistanceTo(unit.transform.position) <= attackRange)
            {
                // only find enemies
                if (unit.player != GameMaster.current.GetCurrentPlayer())
                {
                    enemiesInRange.Add(unit);
                    unit.attackIcon.SetActive(true);
                }
            }
        }
    }

    private void HideAllAttackIcons()
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
            .DOMoveX(position.x, movementSpeed)
            .SetSpeedBased(true)
            .OnComplete(() =>
                transform
                .DOMoveY(position.y, movementSpeed)
                .SetSpeedBased(true)
                .OnComplete(() =>
                {
                    HideAllAttackIcons();
                    GetEnemiesInRange();
                })
            );
        hasMoved = true;

    }

    private void WeakCameraShake()
    {
        cameraAnimator.SetTrigger("WeakShake");
    }

    private void Attack(Unit enemy)
    {

        hasAttacked = true;
        int dealingDamage = attackDamage - enemy.armor;
        int receivingDamage = enemy.defenseDamage - armor;

        if (dealingDamage > 0 || receivingDamage > 0)
        {
            WeakCameraShake();
        }

        if (dealingDamage > 0)
        {
            DamageIcon damageIcon = Instantiate(damageIconPrefab, enemy.transform.position, Quaternion.identity);
            damageIcon.ChooseIconBasedOnDamage(dealingDamage);
            enemy.health -= dealingDamage;
            enemy.UpdateKingHealthText();
        }

        if (receivingDamage > 0)
        {
            DamageIcon damageIcon = Instantiate(damageIconPrefab, transform.position, Quaternion.identity);
            damageIcon.ChooseIconBasedOnDamage(receivingDamage);
            health -= receivingDamage;
            UpdateKingHealthText();
        }

        if (enemy.health <= 0)
        {
            Instantiate(killParticleFx, enemy.transform.position, Quaternion.identity);
            Destroy(enemy.gameObject);
            GetWalkableTiles();
        }

        if (health <= 0)
        {
            Instantiate(killParticleFx, transform.position, Quaternion.identity);
            GameMaster.current.ResetTiles();
            Destroy(this.gameObject);
        }
    }

    public void UpdateKingHealthText()
    {
        if (isKing)
        {
            kingHealthText.text = health.ToString();
        }
    }


    // Update is called once per frame
    void Update()
    {
    }
}

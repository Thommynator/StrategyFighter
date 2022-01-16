using System;
using UnityEngine;
using System.Linq;

public class GameMaster : MonoBehaviour
{

    public static GameMaster current;
    public Unit selectedUnit;
    [SerializeField] private GameObject tileBorderHighlight;
    [SerializeField] private TurnIndicator turnIndicator;

    [Header("Player1")]
    [SerializeField] private PlayerGold playerGold1;
    [SerializeField] private Shop shop1;

    [Header("Player2")]
    [SerializeField] private PlayerGold playerGold2;
    [SerializeField] private Shop shop2;


    void Awake()
    {
        current = this;
    }

    void Start()
    {
        ConfigureShopView();
        GetGoldIncomeFor(Player.PLAYER1);
        GetGoldIncomeFor(Player.PLAYER2);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }

        if (selectedUnit != null)
        {
            tileBorderHighlight.SetActive(true);
            tileBorderHighlight.transform.position = selectedUnit.transform.position;
        }
        else
        {
            tileBorderHighlight.SetActive(false);
        }

    }

    public Player GetCurrentPlayer()
    {
        return turnIndicator.currentPlayer;
    }

    private void GetGoldIncomeFor(Player player)
    {
        int income = FindObjectsOfType<Village>().Where(v => v.owner == player).Sum(v => v.incomePerTurn);
        if (player == Player.PLAYER1)
        {
            playerGold1.IncreaseGold(income);
        }
        else if (player == Player.PLAYER2)
        {
            playerGold2.IncreaseGold(income);
        }

    }

    private void EndTurn()
    {
        turnIndicator.SwitchTurn();

        if (selectedUnit != null)
        {
            selectedUnit.isSelected = false;
            selectedUnit = null;
        }
        ResetTiles();
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.hasMoved = false;
            unit.hasAttacked = false;
            unit.attackIcon.SetActive(false);
        }
        ConfigureShopView();
        GetGoldIncomeFor(GetCurrentPlayer());
    }

    private void ConfigureShopView()
    {
        shop1.SetCurrentPlayer(GetCurrentPlayer());
        shop2.SetCurrentPlayer(GetCurrentPlayer());
    }

    public void ResetTiles()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }

}



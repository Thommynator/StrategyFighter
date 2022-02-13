using System;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class GameMaster : MonoBehaviour
{

    public static GameMaster current;
    public CursorFollower cursorFollower;
    public Unit selectedUnit;
    public ShopItem shopSelectedItem;

    [SerializeField] private GameObject tileBorderHighlight;
    [SerializeField] private TurnIndicator turnIndicator;

    [Header("Player1")]
    [SerializeField] public PlayerGold playerGold1;
    [SerializeField] private Shop shop1;

    [Header("Player2")]
    [SerializeField] public PlayerGold playerGold2;
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

    public bool IsCurrentPlayerByTag(String tag)
    {
        return tag == "Player1" && GetCurrentPlayer() == Player.PLAYER1 || tag == "Player2" && GetCurrentPlayer() == Player.PLAYER2;
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
            unit.currentRemainingAttacks = unit.stats.numberOfAttacks;
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

    public void SetShopSelectedItem(ShopItem shopItem)
    {
        if (selectedUnit != null)
        {
            selectedUnit.isSelected = false;
            selectedUnit = null;
        }
        this.shopSelectedItem = shopItem;

        // FIXME doesn't work yet
        // Texture2D texture = AssetPreview.GetAssetPreview(placeableItem.gameObject);
        // Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        // cursorFollower.SetCursorSprite(sprite);
    }

    public ShopableItem GetAndPayForShopSelectedItem()
    {
        Player player = GetCurrentPlayer();
        if (player == Player.PLAYER1)
        {
            playerGold1.DecreaseGold(shopSelectedItem.costs);
        }
        else if (player == Player.PLAYER2)
        {
            playerGold2.DecreaseGold(shopSelectedItem.costs);
        }
        return shopSelectedItem.shopableItem;
    }


}



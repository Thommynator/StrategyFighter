using System;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster current;
    public CursorFollower cursorFollower;
    public Unit selectedUnit;
    public ShopItem shopSelectedItem;
    public Unit previewUnit;
    public AudioClip celebrationSound;

    [SerializeField] private GameObject tileBorderHighlight;
    [SerializeField] private TurnIndicator turnIndicator;
    [SerializeField] private GameObject winScreen;

    [Header("Player1")]
    [SerializeField] public PlayerGold playerGold1;
    [SerializeField] private Shop shop1;
    [SerializeField] private GameObject winScreenPlayer1;


    [Header("Player2")]
    [SerializeField] public PlayerGold playerGold2;
    [SerializeField] private Shop shop2;
    [SerializeField] private GameObject winScreenPlayer2;


    void Awake()
    {
        current = this;
    }

    void Start()
    {
        winScreen.SetActive(false);
        ConfigureShopView();
        GetGoldIncomeFor(Player.PLAYER1);
        GetGoldIncomeFor(Player.PLAYER2);
        ResetUnitStats();
        ResetTiles();
        GrayOutInactiveUnits();
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

    public Player GetPlayerByTag(String tag)
    {
        return tag == "Player1" ? Player.PLAYER1 : Player.PLAYER2;
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

    public void EndTurn()
    {
        turnIndicator.SwitchTurn();

        ResetSelectedUnit();
        ResetPreviewUnit();
        ResetTiles();
        ResetUnitStats();
        GrayOutInactiveUnits();
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

    private void ResetUnitStats()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.hasMoved = false;
            unit.currentRemainingAttacks = unit.stats.numberOfAttacks;
            unit.attackIcon.SetActive(false);
        }
    }

    private void ResetSelectedUnit()
    {
        if (selectedUnit != null)
        {
            selectedUnit.isSelected = false;
            selectedUnit = null;
        }
    }

    public void ResetPreviewUnit()
    {
        if (previewUnit != null)
        {
            Destroy(previewUnit.gameObject);
            this.previewUnit = null;
        }
    }

    private void GrayOutInactiveUnits()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (IsCurrentPlayerByTag(unit.tag))
            {
                unit.WakeUp();
            }
            else
            {
                unit.Sleep();
            }
        }
    }

    public void SetShopSelectedItem(ShopItem shopItem)
    {
        ResetSelectedUnit();
        ResetPreviewUnit();
        shopSelectedItem = shopItem;

        var unit = Instantiate<Unit>(shopItem.unit);
        unit.gameObject.AddComponent<TileDiscreteMouseFollower>();
        unit.GetComponent<Collider2D>().enabled = false; // disable to prevent that the preview is clickable
        previewUnit = unit;
    }

    public Unit GetAndPayForShopSelectedItem()
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
        return shopSelectedItem.unit;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        winScreen.SetActive(false);
    }

    public void GameOver(Player loser)
    {
        SoundManager.instance.PlayAudio(celebrationSound);
        Player winner = loser.Opposite();
        ShowWinScreen(winner);
    }

    private void ShowWinScreen(Player winner)
    {
        winScreen.SetActive(true);
        if (winner == Player.PLAYER1)
        {
            winScreenPlayer1.SetActive(true);
            winScreenPlayer2.SetActive(false);
        }

        else if (winner == Player.PLAYER2)
        {
            winScreenPlayer1.SetActive(false);
            winScreenPlayer2.SetActive(true);
        }
    }


}



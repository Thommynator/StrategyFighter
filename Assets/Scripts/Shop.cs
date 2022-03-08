using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject shop;
    [SerializeField] private Button toggleButton;
    [SerializeField] private Player shopOwner;
    private bool shouldShow = true;

    void Update()
    {

    }

    public void SetCurrentPlayer(Player currentPlayer)
    {
        bool isPlayerTurn = currentPlayer == shopOwner;
        toggleButton.interactable = isPlayerTurn;
        shop.SetActive(isPlayerTurn && shouldShow);
    }

    public void Toggle()
    {
        shouldShow = !shouldShow;
        shop.SetActive(shouldShow);
    }

    public void PutItemIntoBasket(ShopItem item)
    {
        GameMaster gm = GameMaster.current;
        PlayerGold playerGold = shopOwner == Player.PLAYER1 ? gm.playerGold1 : gm.playerGold2;

        if (item.costs > playerGold.GetGold())
        {
            print($"Not enough gold! {item.unit} costs {item.costs} gold, you only have {playerGold.GetGold()}.");
            return;
        }
        gm.SetShopSelectedItem(item);
        ShowCreateableTiles();
    }

    private void ShowCreateableTiles()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>().Where(tile => tile.IsClear()))
        {
            tile.ShowCreateableHighlight();
        }
    }


}

using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject shop;
    [SerializeField] private Button toggleButton;
    [SerializeField] private Player shopOwner;
    private bool shouldShow = true;


    void Start()
    {

    }

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


}

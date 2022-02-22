using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public Shop shop;
    public int costs;
    private TextMeshProUGUI costsText;
    public Unit unit;


    void Start()
    {
        costsText = GetComponentInChildren<TextMeshProUGUI>();
        costsText.text = costs.ToString();
    }

    public void TryToBuyThisItem()
    {
        shop.PutItemIntoBasket(this);
    }

}

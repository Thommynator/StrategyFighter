using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Shop shop;
    public int costs;
    private TextMeshProUGUI costsText;
    public Unit unit;
    private StatsView statsView;
    private Camera uiCamera;
    private bool isMouseOver;

    void Start()
    {
        isMouseOver = false;
        uiCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
        statsView = FindObjectOfType<StatsView>();
        costsText = GetComponentInChildren<TextMeshProUGUI>();
        costsText.text = costs.ToString();
    }

    void Update()
    {
        if (isMouseOver && Input.GetMouseButtonDown(1))
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector3 position = new Vector3(rectTransform.position.x, rectTransform.position.y, 0);
            statsView.ShowStatsOf(unit, position, true);
        }
    }

    public void TryToBuyThisItem()
    {
        shop.PutItemIntoBasket(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        statsView.HideStats();
    }
}

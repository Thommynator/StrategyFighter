using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayerGold : MonoBehaviour
{
    [SerializeField] private int amount;
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private float changeAnimationDuration;

    public void IncreaseGold(int increase)
    {
        SetGold(amount + increase);
    }

    public void DecreaseGold(int decrease)
    {
        SetGold(amount - decrease);
    }

    private void SetGold(int target)
    {
        DOTween.To(() => amount, x =>
        {
            amount = x;
            goldText.text = amount.ToString();
        },
        target, changeAnimationDuration).SetEase(Ease.InOutQuad);
    }

    public int GetGold()
    {
        return amount;
    }
}

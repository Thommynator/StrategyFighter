using UnityEngine;
using TMPro;

public class StatsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText, armorText, attackDamage, defenseDamage;

    [SerializeField] private Vector2 positionOffset;

    [SerializeField] GameObject visuals;

    public void ShowStatsOf(Unit unit)
    {
        visuals.SetActive(true);
        transform.position = (Vector2)unit.transform.position + positionOffset;

        healthText.text = unit.GetCurrentHealth().ToString();
        armorText.text = unit.stats.armor.ToString();
        attackDamage.text = unit.stats.attackDamage.ToString();
        defenseDamage.text = unit.stats.defenseDamage.ToString();
    }

    public void HideStats()
    {
        visuals.SetActive(false);
    }
}

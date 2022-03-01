using UnityEngine;
using TMPro;

public class StatsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText, armorText, attackDamage, defenseDamage;

    [SerializeField] private Vector2 positionOffset;

    [SerializeField] GameObject visuals;

    public void ShowStatsOf(Unit unit, bool showDefaultHealth = false)
    {
        ShowStatsOf(unit, (Vector2)unit.transform.position, showDefaultHealth);
    }

    public void ShowStatsOf(Unit unit, Vector3 position, bool showDefaultHealth = false)
    {
        visuals.SetActive(true);
        transform.position = position + (Vector3)positionOffset;

        healthText.text = showDefaultHealth ? unit.stats.health.ToString() : unit.GetCurrentHealth().ToString();
        armorText.text = unit.stats.armor.ToString();
        attackDamage.text = unit.stats.attackDamage.ToString();
        defenseDamage.text = unit.stats.defenseDamage.ToString();
    }

    public void HideStats()
    {
        visuals.SetActive(false);
    }
}

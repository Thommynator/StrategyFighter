using TMPro;

public class King : Unit
{
    public TextMeshProUGUI kingHealthText;

    protected override void Start()
    {
        base.Start();
        UpdateKingHealthText();
    }

    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (currentHealth <= 0)
        {
            GameMaster.current.GameOver(GameMaster.current.GetPlayerByTag(tag));
        }
        UpdateKingHealthText();
    }

    private void UpdateKingHealthText()
    {
        kingHealthText.text = currentHealth.ToString();
    }


}

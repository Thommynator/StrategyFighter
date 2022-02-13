using TMPro;

public class King : Unit
{
    public TextMeshProUGUI kingHealthText;

    protected override void Start()
    {
        base.Start();
        UpdateKingHealthText();
    }

    protected override void Attack(Unit enemy)
    {
        base.Attack(enemy);
        UpdateKingHealthText();
        if (currentHealth <= 0)
        {
            print("GAME OVER");
        }
    }

    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        UpdateKingHealthText();
    }

    private void UpdateKingHealthText()
    {
        kingHealthText.text = currentHealth.ToString();
    }


}

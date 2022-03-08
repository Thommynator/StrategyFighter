using TMPro;
using UnityEngine;

public class King : Unit
{
    public TextMeshProUGUI kingHealthText;
    [SerializeField] private ParticleSystem heartParticles;

    protected override void Start()
    {
        base.Start();
        UpdateKingHealthText();
    }

    protected override void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        base.TakeDamage(damage);
        if (currentHealth <= 0)
        {
            GameMaster.current.GameOver(GameMaster.current.GetPlayerByTag(tag));
        }
        heartParticles.Play();
        UpdateKingHealthText();
    }

    private void UpdateKingHealthText()
    {
        kingHealthText.text = currentHealth.ToString();
    }


}

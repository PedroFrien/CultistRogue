using UnityEngine;

public class PlayerHealth : BaseCharacter
{
    private GameManager gameManager;


    private void Start()
    {
        CurrentHealth = MaxHealth;
        gameManager = FindFirstObjectByType<GameManager>();
    }
    public override void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0 )
        {
            Die();
        }
    }

    public override void Die()
    {
        gameManager.LoadScene("DeathScreen");
    }
}

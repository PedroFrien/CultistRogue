using UnityEngine;

public class PlayerHealth : BaseCharacter
{
    private GameManager gameManager;


    [SerializeField] private Healthbar healthBar;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        gameManager = FindFirstObjectByType<GameManager>();
    }
    public override void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        float healthValue = CurrentHealth / MaxHealth;

        healthBar.ChangeValue(healthValue);

        Debug.Log(CurrentHealth);


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

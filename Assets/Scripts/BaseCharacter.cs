using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    public float CurrentHealth;
    public float MaxHealth;



    public virtual void TakeDamage(float damageTaken)
    {
        CurrentHealth -= damageTaken;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Heal() { }
    
    






}

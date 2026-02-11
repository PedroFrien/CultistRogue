using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    public float CurrentHealth;
    public float MaxHealth;



    public abstract void TakeDamage(float damageTaken);

    public abstract void Die();

    public virtual void Heal() { }
    
    






}

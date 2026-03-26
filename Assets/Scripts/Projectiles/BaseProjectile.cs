using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    public float damage;


    public void OnCollisionEnter(Collision other)
    {
        OnCollide(other);
    }

    public virtual void OnCollide(Collision collider)
    {
        BaseCharacter character = collider.gameObject.GetComponent<BaseCharacter>();
        if (character != null)
        {
            character.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}

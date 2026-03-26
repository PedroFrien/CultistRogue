using UnityEngine;

public class Arrow : BaseProjectile
{
    private bool damageActive = true;

    private void Awake()
    {
        damageActive = false;

        Invoke("ActivateDamage",0.1f);
    }
    public override void OnCollide(Collision collision)
    {
        BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            Destroy(gameObject);
        }
        if (player != null && damageActive == true)
        {
            player.TakeDamage(damage);
        }
        else
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;


            ContactPoint contact = collision.GetContact(0);
            transform.rotation = Quaternion.LookRotation(-contact.normal);

            transform.SetParent(collision.transform);

            damageActive = false;
        }
    }

    private void ActivateDamage()
    {
        damageActive = true;
    }
}

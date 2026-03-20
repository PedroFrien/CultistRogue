using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour, IInteractable
{
    public float damage;

    public Vector3 offsetPos;
    public Vector3 rotateOffsetPos;
    public float attackInterval;
    public bool canAttack = true;


    public float noiseRadius;

    public bool equipped;


    public virtual void Use()
    {
        if (!equipped || !canAttack) return;
        canAttack = false;

        Attack();

        Invoke("AttackReset", attackInterval);
    }

    public abstract void Attack();

    public void AttackReset()
    {
        canAttack = true;
    }

    public void OnInteract()
    {
        FindFirstObjectByType<WeaponManager>().PickupWeapon(this);
    }

    


}

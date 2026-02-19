using UnityEngine;

public abstract class BaseMelee : BaseWeapon
{
    public float range;
    public Camera mainCamera;
    public Animator animator;
    public LayerMask meleeMask;
    public float attackInterval;
    public bool canAttack = true;


    private void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }
    public override void Use()
    {
        if (!canAttack || FindFirstObjectByType<GameManager>().menuOpen) return;
        canAttack = false;

        animator.SetTrigger("Attack");

        FindFirstObjectByType<AudioManager>().PlaySound("Stab", transform.position, gameObject);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, mainCamera.transform.forward, out hit, range, meleeMask)) 
        {
            BaseCharacter character = hit.collider.GetComponent<BaseCharacter>();
            if (character != null)
            {
                character.TakeDamage(damage);
            }
        }

        Invoke("AttackReset", attackInterval);

        
    }

    public void AttackReset()
    {
        canAttack = true;
    }
}

using UnityEngine;

public abstract class BaseMelee : BaseWeapon
{
    public float range;
    public Camera mainCamera;
    public Animator animator;
    public LayerMask meleeMask;
    
    


    private void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }
    public override void Attack()
    {
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


    }

    
}

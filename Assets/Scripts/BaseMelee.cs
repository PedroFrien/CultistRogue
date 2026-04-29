using UnityEngine;

public abstract class BaseMelee : BaseWeapon
{
    public float range;
    public Camera mainCamera;
    public Animator animator;
    public LayerMask meleeMask;

    public FPController controller;

    public bool backstab = false;
    
    


    private void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        controller = FindFirstObjectByType<FPController>();
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, mainCamera.transform.forward, out hit, range, meleeMask))
        {
            if (hit.collider.CompareTag("Backstab"))
            {
                backstab = true;
                animator.SetBool("Backstab", true);
                Debug.Log("Backstabbing Range");
            }
            else
            {
                backstab = false;
                animator.SetBool("Backstab", false);
                Debug.Log("No Backstab");
            }
        }
        else
        {
            backstab = false;
            animator.SetBool("Backstab", false);
        }
    }
    public override void Attack()
    {
        animator.SetTrigger("Attack");

        FindFirstObjectByType<AudioManager>().PlaySound("Stab", transform.position, gameObject);

        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, mainCamera.transform.forward, out hit, range, meleeMask)) 
        {
            BaseCharacter character = null;
            if (backstab == false)
            {
                character = hit.collider.GetComponent<BaseCharacter>();
            }
            else
            {
                character = hit.collider.GetComponentInParent<BaseCharacter>();
            }
   
            if (character != null)
            {
                if (backstab == false)
                {
                    character.TakeDamage(damage);
                }
                if (backstab == true)
                {
                    character.TakeDamage(damage*999);
                }


                
            }
        }


    }

    
}

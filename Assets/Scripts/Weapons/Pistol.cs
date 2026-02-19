using Unity.VisualScripting;
using UnityEngine;

public class Pistol : BaseGun
{
    
    public override void Use()
    {
        if (FindFirstObjectByType<GameManager>().menuOpen) return;


        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo, not able to use");
            return;
        }

        //animator.SetBool("Shoot", false);
        //animator.SetBool("Shoot", true);

        FindFirstObjectByType<AudioManager>().PlaySound("Gunshot", transform.position, gameObject);
        animator.SetTrigger("Shoot");

        

        Ray ray = mainCamera.ScreenPointToRay(ScreenCenter());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, hitLayer))
        {
            BaseCharacter character = hit.collider.GetComponent<BaseCharacter>();
            if (character != null)
            {
                character.TakeDamage(damage);
            }

            SpawnTrail(hit.point);

            if (hit.collider.tag == "Environment") SpawnDecal(hit);

        }
        else
        {
            SpawnTrail(ray.GetPoint(range));
        }

        


        DecreaseAmmo();

    }
}

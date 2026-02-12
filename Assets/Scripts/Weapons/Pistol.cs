using Unity.VisualScripting;
using UnityEngine;

public class Pistol : BaseGun
{




    public override void Use()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo, not able to use");
            return;
        }

        

        Ray ray = mainCamera.ScreenPointToRay(ScreenCenter());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
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

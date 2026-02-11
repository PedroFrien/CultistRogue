using UnityEngine;

public class Pistol : BaseWeapon
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float damage;



    public override void Use()
    {
        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, Vector3.forward, out hit, 99))
        {
            BaseCharacter character = hit.collider.GetComponent<BaseCharacter>();
            if (character != null)
            {
                character.TakeDamage(damage);
            }
        }
       
    }
}

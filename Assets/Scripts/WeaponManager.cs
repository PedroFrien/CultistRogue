using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<BaseWeapon> startingWeapons = new List<BaseWeapon>();
    public List<BaseWeapon> heldWeapons = new List<BaseWeapon>();

    public BaseWeapon currentWeapon;

    [SerializeField] private Transform weaponHoldPos;


    public void Start()
    {
        heldWeapons.Clear();


        if (startingWeapons.Count > 0 && currentWeapon == null)
        {            
            foreach (var weapon in startingWeapons)
            {
                BaseWeapon spawnedWeapon = Instantiate(weapon, weaponHoldPos.position, weaponHoldPos.rotation);


                spawnedWeapon.transform.parent = weaponHoldPos;
                //spawnedWeapon.transform.localPosition = Vector3.zero;
                //spawnedWeapon.transform.localRotation = Quaternion.identity;


                heldWeapons.Add(spawnedWeapon);
                spawnedWeapon.gameObject.SetActive(false);
            }

            currentWeapon = heldWeapons[0];

            currentWeapon.gameObject.SetActive(true);
            currentWeapon.GetComponent<Rigidbody>().isKinematic = true;

        }
    }


    public void SwapWeapon(int index)
    {
        currentWeapon.gameObject.SetActive(false);
        currentWeapon = heldWeapons[index];
        currentWeapon.gameObject.SetActive(true);
    }

    public void UseWeapon()
    {
        if (currentWeapon == null)
        {
            Debug.Log("No Weapon Equipped");
        }
        else
        {
            currentWeapon.Use();
        }
    }

    public void ReloadWeapon()
    {
        BaseGun gun = currentWeapon.GetComponent<BaseGun>();

        if (gun != null)
        {
            gun.StartCoroutine("Reload");
        }

        
    }

    


}

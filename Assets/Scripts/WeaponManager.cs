using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<BaseWeapon> startingWeapons = new List<BaseWeapon>();
    public List<BaseWeapon> heldWeapons = new List<BaseWeapon>();

    public BaseWeapon currentWeapon;

    [SerializeField] private Transform weaponHoldPos;

    [SerializeField] private TMP_Text ammoCount;


    public void Start()
    {
        heldWeapons.Clear();


        if (startingWeapons.Count > 0 && currentWeapon == null)
        {            
            foreach (var weapon in startingWeapons)
            {
                BaseWeapon spawnedWeapon = Instantiate(weapon, weaponHoldPos.position, weaponHoldPos.rotation);


                spawnedWeapon.transform.parent = weaponHoldPos;
                spawnedWeapon.transform.localPosition += spawnedWeapon.offsetPos;
                spawnedWeapon.transform.localRotation = Quaternion.Euler(spawnedWeapon.rotateOffsetPos);
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

        GunUpdate();
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

        GunUpdate();
        
    }

    public void GunUpdate()
    {
        BaseGun gun = currentWeapon.GetComponent<BaseGun>();
        if (gun != null)
        {
            ammoCount.gameObject.SetActive(true);
            ammoCount.text = gun.currentAmmo + " / " + gun.reserveAmmo;
        }
        else
        {
            ammoCount.gameObject.SetActive(false);
        }
    }

    public void ReloadWeapon()
    {
        BaseGun gun = currentWeapon.GetComponent<BaseGun>();

        if (gun != null)
        {
            gun.StartCoroutine("Reload");
        }

        GunUpdate();

        
    }

    


}

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Transactions;
using UnityEngine.Jobs;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<BaseWeapon> startingWeapons = new List<BaseWeapon>();
    public List<BaseWeapon> heldWeapons = new List<BaseWeapon>();

    public BaseWeapon currentWeapon;
    public int currentWeaponIndex = 0;
    public int maxWeapons;

    [SerializeField] private Transform weaponHoldPos;

    [SerializeField] private TMP_Text ammoCount;

    //private void Update()
    //{

    //}
    public void Start()
    {
        heldWeapons.Clear();


        if (startingWeapons.Count > 0 && currentWeapon == null)
        {            
            foreach (var weapon in startingWeapons)
            {
                BaseWeapon spawnedWeapon = Instantiate(weapon, weaponHoldPos.position, weaponHoldPos.rotation);


                spawnedWeapon.transform.parent = weaponHoldPos;
                spawnedWeapon.transform.localPosition = spawnedWeapon.offsetPos;
                spawnedWeapon.transform.localRotation = Quaternion.Euler(spawnedWeapon.rotateOffsetPos);



                spawnedWeapon.GetComponent<Rigidbody>().isKinematic = true;
                spawnedWeapon.GetComponent<BoxCollider>().enabled = false;

                heldWeapons.Add(spawnedWeapon);
                spawnedWeapon.equipped = true;
                spawnedWeapon.gameObject.SetActive(false);
            }

            currentWeapon = heldWeapons[0];
            currentWeaponIndex = 0;

       

            SwapWeapon(currentWeaponIndex);
            
            

        }

    }


    public void SwapWeapon(int index)
    {
        if (currentWeapon != null)
        {
            BaseGun potentialGun = currentWeapon.GetComponent<BaseGun>();
            if (potentialGun != null && potentialGun.reloading == true) return;
        }
        
        
        if (heldWeapons.Count >= index + 1 && heldWeapons[index] != null)
        {

            if (currentWeapon != null)
            {
                currentWeapon.gameObject.SetActive(false);
            }           
            currentWeapon = heldWeapons[index];
            currentWeapon.gameObject.SetActive(true);
            currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
            currentWeapon.GetComponent<BoxCollider>().enabled = false;
            

            currentWeaponIndex = index;
            currentWeapon = heldWeapons[index];

            GunUpdate();
        }
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
            GunUpdate();
        }

        
        
    }

    public void GunUpdate()
    {
        if (currentWeapon != null)
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

    public void PickupWeapon(BaseWeapon weapon)
    {
        if (heldWeapons.Count == maxWeapons)
        {
            DropWeapon();
        }





        weapon.transform.parent = weaponHoldPos;
        weapon.transform.parent = weaponHoldPos;
        weapon.transform.localPosition = Vector3.zero;      
        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.localPosition = weapon.offsetPos;
        weapon.transform.localRotation = Quaternion.Euler(weapon.rotateOffsetPos);
        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.GetComponent<BoxCollider>().enabled = false;



        heldWeapons.Add(weapon);
        //weapon.gameObject.SetActive(false);

        weapon.equipped = true;
        int weaponIndex = heldWeapons.Count - 1;

        SwapWeapon(weaponIndex);

    }

    public void DropWeapon()
    {
        if (currentWeapon == null) return;

        BaseWeapon weapon = heldWeapons[currentWeaponIndex];

        if (weapon != null)
        {
            weapon.gameObject.SetActive(true);
            weapon.transform.parent = null;
            weapon.GetComponent<Rigidbody>().isKinematic = false;
            weapon.GetComponent<BoxCollider>().enabled = true;

            weapon.GetComponent<BaseWeapon>().enabled = false;

            
        }

        currentWeapon = null;
        
        //weapon.transform.localPosition += weapon.offsetPos;
        //weapon.transform.localRotation = Quaternion.Euler(weapon.rotateOffsetPos);



        weapon.equipped = false;

        heldWeapons.Remove(weapon);
       
        if (heldWeapons.Count < currentWeaponIndex + 1)
        {
            currentWeaponIndex -= 1;
            currentWeaponIndex = Mathf.Clamp(currentWeaponIndex, 0, heldWeapons.Count + 1);
        }

        SwapWeapon(currentWeaponIndex);




    }

    


}

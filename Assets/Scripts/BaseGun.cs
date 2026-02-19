using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public abstract class BaseGun : BaseWeapon
{
    public int currentAmmo;
    public int magSize;
    public int reserveAmmo;

    public float fireInterval;
    public float range;

    public float reloadTime;

    public Transform FirePoint;

    public GameObject bulletTrail;
    public float trailDuration;
    public GameObject decal;
    public float decalDuration;

    public Camera mainCamera;

    public Animator animator;

    public LayerMask hitLayer;

    public bool reloading = false;



    private void Start()
    {
        mainCamera = Camera.main;
        Debug.Log("Calling Start");
        animator = GetComponent<Animator>();
    }
    public virtual void DecreaseAmmo()
    {
        currentAmmo -= 1;
    }

    public virtual void StartReload()
    {
        StartCoroutine(Reload());
    }
    public IEnumerator Reload()
    {
        if (currentAmmo >= magSize || reserveAmmo <= 0 || reloading)
        { 
            yield break;
        }

        reloading = true;

        FindFirstObjectByType<AudioManager>().PlaySound("Reload", transform.position, gameObject);

        animator.SetTrigger("Reload");

        Debug.Log("Reload");

        
        

        yield return new WaitForSeconds(reloadTime);

        if (reserveAmmo >= magSize)
        {
            currentAmmo = magSize;
            reserveAmmo -= magSize;
        }
        else
        {
            currentAmmo = reserveAmmo;
            reserveAmmo = 0;
        }

        reloading = false;

        FindFirstObjectByType<WeaponManager>().GunUpdate();
    }

    public Vector3 ScreenCenter()
    {
        float centerX = Screen.width / 2f;
        float centerY = Screen.height / 2f;
        Vector3 screenCenter = new Vector3(centerX, centerY, 0);
        return screenCenter;
    }

    public void SpawnTrail(Vector3 hitPoint)
    {
        if (bulletTrail == null)
        {
            Debug.Log("No Trail set! Returning...");
            return;
        }

        GameObject line = Instantiate(bulletTrail, FirePoint.position, Quaternion.identity);
        LineRenderer lineRen = line.GetComponent<LineRenderer>();

        lineRen.useWorldSpace = true;
        lineRen.SetPosition(0, FirePoint.position);
        lineRen.SetPosition(1, hitPoint);

        //Destroy(line, trailDuration);
    }

    public void SpawnDecal(RaycastHit hit)
    {
        if (decal == null)
        {
            Debug.Log("No Decal set! Returning...");
            return;
        }

        float offsetDistance = 0.01f; 
        Vector3 offsetPosition = hit.point + hit.normal * offsetDistance;

        if (hit.collider != null && hit.collider.tag == "Environment")
        {
            GameObject spawnedDecal = Instantiate(decal, offsetPosition, Quaternion.LookRotation(hit.normal));

            //Destroy(spawnedDecal, decalDuration);
        }

        
    }
}

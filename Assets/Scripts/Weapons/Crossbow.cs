using UnityEngine;

public class Crossbow : BaseGun
{
    [SerializeField] private BaseProjectile arrow;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private MeshRenderer visualArrow;


    public override void Attack()
    {
        animator.SetTrigger("Shoot");
        FindFirstObjectByType<AudioManager>().PlaySound("CrossbowShot", transform.position, gameObject);

        BaseProjectile spawnedArrow = Instantiate(arrow, FirePoint.position, FirePoint.rotation);


        visualArrow.enabled = false;
        Vector3 dir = mainCamera.transform.forward;
        spawnedArrow.GetComponent<Rigidbody>().linearVelocity = dir * arrowSpeed;


        DecreaseAmmo();

    }

    public override void StartReload()
    {

        visualArrow.enabled = true;
        StartCoroutine(Reload());

        
    }
}

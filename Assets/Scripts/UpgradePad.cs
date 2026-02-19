using System.Collections;
using UnityEngine;

public class UpgradePad : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private ParticleSystem upgradeParticles;


    private void OnTriggerEnter(Collider other)
    {
        AbilityPickup pickup = other.GetComponent<AbilityPickup>();

        if (pickup != null)
        {
            StartCoroutine(UpgradeAbility(pickup.instantiatedAbility));
        }
    }

    private IEnumerator UpgradeAbility(BaseAbility ability)
    {
        FindFirstObjectByType<AudioManager>().PlaySound("UpgradeStart", transform.position, gameObject);

        yield return new WaitForSeconds(delay);

        FindFirstObjectByType<AudioManager>().PlaySound("Upgrade", transform.position, gameObject);

        ability.upgraded = true;

        Instantiate(upgradeParticles, transform.position, Quaternion.identity);

        Destroy(gameObject.transform.parent.gameObject);
    }


}

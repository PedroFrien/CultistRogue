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
        yield return new WaitForSeconds(delay);

        ability.upgraded = true;

        Instantiate(upgradeParticles, transform.position, Quaternion.identity);

        Destroy(gameObject.transform.parent.gameObject);
    }


}

using UnityEngine;

public class AbilityPickup : MonoBehaviour, IInteractable
{
    public BaseAbility ability;

    public BaseAbility instantiatedAbility;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ability != null)
        {
            instantiatedAbility = Instantiate(ability);
        }
        else
        {
            instantiatedAbility = ability;
        }

    
    }


    public void OnInteract()
    {
        AbilityManager abilityManager = FindFirstObjectByType<AbilityManager>();

        if (abilityManager.equippedAbilities.Count >= abilityManager.maxAbilities)
        {
            Debug.Log("Max Abilities Reached!");
        }
        else
        {
            abilityManager.AddAbility(instantiatedAbility);

            Destroy(gameObject);
        }

        
    }
}

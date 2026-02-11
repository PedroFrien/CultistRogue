using UnityEngine;

public class AbilityWheel : MonoBehaviour
{
    public BaseAbility hoveredAbility;

    private AbilityManager abilityManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilityManager = FindFirstObjectByType<AbilityManager>();

        abilityManager.abilityChange.AddListener(RemoveHover);

        gameObject.SetActive(false);
    }

    public void HoverAbility(int abilityIndex)
    {
        if (abilityManager.equippedAbilities.Count > abilityIndex)
        {
            hoveredAbility = abilityManager.equippedAbilities[abilityIndex];
        }
        
    }

    public void RemoveHover()
    {
        hoveredAbility = null;
    }

    public void ActivateHoveredAbility() 
    {
        if (hoveredAbility != null)
        {
            hoveredAbility.OnActivate();
        }

        hoveredAbility = null;
        
    }
}

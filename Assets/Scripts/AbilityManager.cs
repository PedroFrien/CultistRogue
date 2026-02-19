using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AbilityManager : MonoBehaviour
{
    public List<BaseAbility> equippedAbilities = new List<BaseAbility>();

    public List<BaseAbility> onCooldown = new List<BaseAbility>();

    [SerializeField] private AbilityPickup abilityPickup;

    public UnityEvent abilityChange = new UnityEvent();
    public UnityEvent incrementCoolown = new UnityEvent();

    public int maxAbilities = 5;

    #region Ability Management
    public void AddAbility(BaseAbility ability)
    {
        equippedAbilities.Add(ability);

        abilityChange.Invoke();

        ability.OnStart();


    }
    public void ActivateAbility(int abilityIndex)
    {
        BaseAbility ability = equippedAbilities[abilityIndex];

        ability.OnActivate();

        
    }

    public void UpgradeAbility(int abilityIndex)
    {
        BaseAbility ability = equippedAbilities[abilityIndex];

        ability.upgraded = true;

        abilityChange.Invoke();
    }

    public void DropAbility(int abilityIndex)
    {
        if (equippedAbilities.Count > abilityIndex)
        {
            FindFirstObjectByType<AudioManager>().PlaySound("DropAbility", transform.position, gameObject);

            BaseAbility removedAbility = equippedAbilities[abilityIndex];

            equippedAbilities.RemoveAt(abilityIndex);

            Vector3 spawnPos = transform.position + transform.forward * 1f;
            spawnPos.y = transform.position.y;

            AbilityPickup spawnedPickup = Instantiate(abilityPickup, spawnPos, Quaternion.identity);

            spawnedPickup.ability = removedAbility;

            abilityChange.Invoke();
        }        
    }
    #endregion

    private void Update()
    {
        incrementCoolown.Invoke();


        //foreach (BaseAbility ability in onCooldown)
        //{
        //    ability.IncrementCooldown();
        //    if (ability.cooldownTime >= ability.cooldown)
        //    {
        //        onCooldown.Remove(ability);
        //    }
        //}
    }

}

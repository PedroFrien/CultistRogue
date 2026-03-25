using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AbilityManager : MonoBehaviour
{
    public float mana;
    public float maxMana = 100f;
    [SerializeField] private Healthbar manaBar;

    public List<BaseAbility> equippedAbilities = new List<BaseAbility>();

    public List<BaseAbility> onCooldown = new List<BaseAbility>();

    [SerializeField] private AbilityPickup abilityPickup;

    public UnityEvent abilityChange = new UnityEvent();
    public UnityEvent incrementCoolown = new UnityEvent();

    public BaseAbility lastAbility;

    public int maxAbilities = 5;


    [Header("Mana Regeneration")]
    [SerializeField] private float manaRegenDelay;
    [SerializeField] private float manaRegenSpeed;
    [SerializeField] private float manaRegenDuration;
    private Coroutine manaRegen;

    #region Ability Management

    private void Start()
    {
        mana = maxMana;
    }
    public void AddAbility(BaseAbility ability)
    {
        equippedAbilities.Add(ability);

        abilityChange.Invoke();

        ability.OnStart();


    }
    public void ActivateAbility(BaseAbility ability)
    {
        Debug.Log("ActivateAbility called");
        //BaseAbility ability = equippedAbilities[abilityIndex];

        if (mana >= ability.manaCost)
        {
            Debug.Log("Activating Ability");
            ability.OnActivate();

            lastAbility = ability;


            GainMana(-ability.manaCost);

            Debug.Log(ability.manaCost);

            if (manaRegen != null)
            {
                StopCoroutine(manaRegen);
            }

            manaRegen = StartCoroutine(RegenerateMana(manaRegenDuration));
        }

        Debug.Log("Can't use ability. No mana!");
        
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

    public void GainMana(float valueGained)
    {
        mana += valueGained;

        float barValue = mana / maxMana;

        manaBar.ChangeValue(barValue);
        mana = Mathf.Clamp(mana, 0, maxMana);
    }

    private IEnumerator RegenerateMana(float duration)
    {
        if (lastAbility == null) yield break;

        yield return new WaitForSeconds(manaRegenDelay);

        float manaToRestore = lastAbility.manaCost;
        float elapsed = 0f;
        float regenRate = manaToRestore / duration; // mana per second

        while (elapsed < duration)
        {
            float delta = regenRate * Time.deltaTime;
            elapsed += Time.deltaTime;

            mana = Mathf.Clamp(mana + delta, 0, maxMana);
            manaBar.ChangeValue(mana / maxMana);

            yield return null;
        }
    }

}

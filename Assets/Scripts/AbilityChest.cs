using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class AbilityChest : MonoBehaviour, IInteractable
{
    [SerializeField] private List<BaseAbility> possibleAbilities;
    [SerializeField] private AbilityPickup pickupPrefab;
    [SerializeField] private Transform spawnPos;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnInteract()
    {
        Open();
    }

    private void Open()
    {
        GetComponent<BoxCollider>().enabled = false;

        int randomIndex = Random.Range(0, possibleAbilities.Count);

        BaseAbility ability = possibleAbilities[randomIndex];

        AbilityPickup pickup = Instantiate(pickupPrefab, spawnPos.position, Quaternion.identity);
        pickup.ability = ability;
        pickup.instantiatedAbility = ability;

        animator.SetTrigger("Open");


    }

    
}

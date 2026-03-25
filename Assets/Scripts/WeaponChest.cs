using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class WeaponChest : MonoBehaviour, IInteractable
{
    [SerializeField] private List<BaseWeapon> possibleWeapons;
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

        int randomIndex = Random.Range(0, possibleWeapons.Count);

        BaseWeapon weapon = possibleWeapons[randomIndex];

        Instantiate(weapon, spawnPos.position, Quaternion.identity);

        animator.SetTrigger("Open");

     


    }

    
}

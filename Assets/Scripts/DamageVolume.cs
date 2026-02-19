using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class DamageVolume : MonoBehaviour
{

    [SerializeField] private float damage;

    [SerializeField] private List<BaseCharacter> overlappingCharacters = new List<BaseCharacter>();

    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (gameManager.paused) return;
        foreach (BaseCharacter character in overlappingCharacters)
        {
            character.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        BaseCharacter character = other.GetComponent<BaseCharacter>();

        if (character != null)
        {
            overlappingCharacters.Add(character); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BaseCharacter character = other.GetComponent<BaseCharacter>();

        if (character != null)
        {
            overlappingCharacters.Remove(character);
        }
    }
}

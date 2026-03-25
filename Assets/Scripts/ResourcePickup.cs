using System.Runtime.InteropServices;
using UnityEngine;

public class ResourcePickup : MonoBehaviour, IInteractable
{
    [SerializeField] private float health;
    [SerializeField] private float mana;

    private AbilityManager pmana;
    private PlayerHealth phealth;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pmana = FindFirstObjectByType<AbilityManager>();
        phealth = FindFirstObjectByType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        if (health > 0)
        {
            Debug.Log("Giving health");
            phealth.Heal(health);
        }

        if (mana > 0)
        {
            Debug.Log("Giving mana");
            pmana.GainMana(mana);
        }

        Destroy(gameObject);


    }


}

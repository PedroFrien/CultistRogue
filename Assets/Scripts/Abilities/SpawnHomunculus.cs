using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "SpawnHomunculus", menuName = "Abilities/SpawnHomunculus")]
public class SpawnHomunculus : BaseAbility
{
    private FPController controller;

    [SerializeField] private Homunculus homunculus;





    public override void OnStart()
    {
        controller = FindFirstObjectByType<FPController>();
        AbilityManager abilityManager = FindFirstObjectByType<AbilityManager>();
        abilityManager.incrementCoolown.AddListener(IncrementCooldown);
    }
    

    public override void Activate()
    {
        Homunculus spawnedHomunculus = Instantiate(homunculus, controller.transform.position, Quaternion.identity);
        
        if (upgraded)
        {
            spawnedHomunculus.speed *= 2;
            spawnedHomunculus.explosionRange *= 1.5f;
            spawnedHomunculus.GetComponent<NavMeshAgent>().speed = spawnedHomunculus.speed;
        }

    }

}


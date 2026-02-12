using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TestDistract", menuName = "Abilities/TestDistract")]
public class TestDistract : BaseAbility
{
    public float range;
    public float upgradedRange;
    private Transform player;


    public override void OnStart()
    {
        AbilityManager abilityManager = FindFirstObjectByType<AbilityManager>();
        abilityManager.incrementCoolown.AddListener(IncrementCooldown);
        player = GameObject.FindWithTag("Player").transform;
    }
    public override void OnActivate()
    {
        if (onCooldown) return;

        if (!upgraded)
        {
            Collider[] colliders = Physics.OverlapSphere(player.position, range);

            foreach (Collider collider in colliders)
            {
                BaseEnemy enemy = collider.GetComponent<BaseEnemy>();
                if (enemy != null) 
                {
                    Debug.Log("Trying to distract enemy");
                    enemy.Investigate(player.position);
                }
            }



        }

        if (upgraded)
        {
            Collider[] colliders = Physics.OverlapSphere(player.position, upgradedRange);

            foreach (Collider collider in colliders)
            {
                BaseEnemy enemy = collider.GetComponent<BaseEnemy>();
                if (enemy != null)
                {
                    enemy.Investigate(player.position);
                }
            }



        }

        onCooldown = true;

    }


    
}

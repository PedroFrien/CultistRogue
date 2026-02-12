using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Abilities/Dash")]
public class Dash : BaseAbility
{
    private FPController controller;

    [SerializeField] private float force;
    [SerializeField] private float upgradedForce;





    public override void OnStart()
    {
        AbilityManager abilityManager = FindFirstObjectByType<AbilityManager>();
        abilityManager.incrementCoolown.AddListener(IncrementCooldown);

        controller = GameObject.FindFirstObjectByType<FPController>();
    }

    public override void OnActivate()
    {
        if (onCooldown) return;

        if (!upgraded)
        {
            float storedVelocity = force;
            controller.VerticalVelocity = 0f;

            Vector3 lookDir = controller.transform.forward;

            controller.AddVelocity(storedVelocity, lookDir);
        }
        if (upgraded)
        {
            float storedVelocity = upgradedForce;
            controller.VerticalVelocity = 0f;

            Vector3 lookDir = controller.transform.forward;

            controller.AddVelocity(storedVelocity, lookDir);
        }

        onCooldown = true;


    }

}


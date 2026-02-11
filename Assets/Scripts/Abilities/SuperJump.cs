using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperJump", menuName = "Abilities/SuperJump")]
public class SuperJump : BaseAbility
{
    private FPController controller;

    [SerializeField] private float jumpForce;
    [SerializeField] private float upgradedJumpForce;




    public override void OnStart()
    {
        AbilityManager abilityManager = FindFirstObjectByType<AbilityManager>();
        abilityManager.incrementCoolown.AddListener(IncrementCooldown);
    }

    public override void OnActivate()
    {
        if (onCooldown) return;

        controller = FindFirstObjectByType<FPController>();


        if (!upgraded)
        {
            controller.VerticalVelocity += jumpForce;
        }

        if (upgraded)
        {
            controller.VerticalVelocity += upgradedJumpForce;
        }

        onCooldown = true;
            
    }
}

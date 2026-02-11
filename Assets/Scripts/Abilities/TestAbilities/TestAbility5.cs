using UnityEngine;



[CreateAssetMenu(fileName = "TestAbility5", menuName = "Abilities/TestAbility5")]
public class TestAbility5 : BaseAbility
{

    public override void OnActivate()
    {
        if (!upgraded)
        {
            Debug.Log("TestAbility5");
        }

        if (upgraded)
        {
            Debug.Log("UPGRADED TestAbility5");
        }
    }
}


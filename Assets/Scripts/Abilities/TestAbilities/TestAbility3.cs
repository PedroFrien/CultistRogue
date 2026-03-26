using UnityEngine;



[CreateAssetMenu(fileName = "TestAbility3", menuName = "Abilities/TestAbility3")]
public class TestAbility3 : BaseAbility
{

    public override void Activate()
    {
        if (!upgraded)
        {
            Debug.Log("TestAbility3");
        }

        if (upgraded)
        {
            Debug.Log("UPGRADED TestAbility3");
        }
    }
}


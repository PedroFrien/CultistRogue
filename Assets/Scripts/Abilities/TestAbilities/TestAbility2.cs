using UnityEngine;



[CreateAssetMenu(fileName = "TestAbility2", menuName = "Abilities/TestAbility2")]
public class TestAbility2 : BaseAbility
{

    public override void OnActivate()
    {
        if (!upgraded)
        {
            Debug.Log("TestAbility2");
        }

        if (upgraded)
        {
            Debug.Log("UPGRADED TestAbility2");
        }
    }
}


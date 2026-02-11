using UnityEngine;



[CreateAssetMenu(fileName = "TestAbility1", menuName = "Abilities/TestAbility1")]
public class TestAbility1 : BaseAbility
{
    
    public override void OnActivate()
    {
        if (!upgraded)
        {
            Debug.Log("TestAbility1");
        }

        if (upgraded)
        {
            Debug.Log("UPGRADED TestAbility1");
        }
    }
}


using UnityEngine;



[CreateAssetMenu(fileName = "TestAbility4", menuName = "Abilities/TestAbility4")]
public class TestAbility4 : BaseAbility
{

    public override void OnActivate()
    {
        if (!upgraded)
        {
            Debug.Log("TestAbility4");
        }

        if (upgraded)
        {
            Debug.Log("UPGRADED TestAbility4");
        }
    }
}


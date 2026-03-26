using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnHomunculus", menuName = "Abilities/SpawnHomunculus")]
public class SpawnHomunculus : BaseAbility
{
    private FPController controller;

    [SerializeField] private GameObject homunculus;





    public override void OnStart()
    {
        controller = FindFirstObjectByType<FPController>();
    }
    

    public override void Activate()
    {
        Instantiate(homunculus, controller.transform.position, Quaternion.identity);
        


    }

}


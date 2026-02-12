using UnityEngine;

public class TestEnemy : BaseEnemy
{
    private void Update()
    {
        StateUpdate();
    }




    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Something triggered");
    //    if (other.GetComponent<FPController>() != null)
    //    {
    //        Debug.Log("Player Triggered");
    //        StartChase();
    //    }
    //}
}

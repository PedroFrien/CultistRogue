using UnityEngine;

public class TestViewCone : MonoBehaviour
{
    [SerializeField] private TestEnemy testEnemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FPController>() != null)
        {
            testEnemy.StartChase();
        }


    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.GetComponent <FPController>() != null)
    //    {
    //        testEnemy.Chasing = false;
    //    }
    //}
}

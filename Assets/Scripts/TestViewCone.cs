using Unity.VisualScripting;
using UnityEngine;

public class TestViewCone : MonoBehaviour
{
    [SerializeField] private TestEnemy testEnemy;
    private BaseEnemy attachedEnemy;

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FPController>() != null)
        {
            testEnemy.playerInCone = true;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FPController>() != null)
        {
            testEnemy.playerInCone = false;
        }


    }

    private void Update()
    {
        if (testEnemy.Chasing)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
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

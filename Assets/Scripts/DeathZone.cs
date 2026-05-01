using UnityEngine;

public class DeathZone : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.Die();
        }
    }
}

using UnityEngine;

public class DeathZone : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = FindFirstObjectByType<PlayerHealth>();
        if (player != null)
        {
            player.Die();
        }
    }
}

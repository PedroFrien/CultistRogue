using UnityEngine;

public class WinVolume : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        FindFirstObjectByType<AudioManager>().PlaySound("Win", transform.position, gameObject);
        gameManager.LoadScene("WinScreen");
    }
}

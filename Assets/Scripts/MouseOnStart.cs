using UnityEngine;

public class MouseOnStart : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private bool mouseOnStart = true;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.SetPause(false);

        if (mouseOnStart)
        { 
            gameManager.SetMouseActive(true);
        }
        else
        {
            gameManager.SetMouseActive(false);
        }
        


    }
}

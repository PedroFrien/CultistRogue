using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool paused = false;
    public bool menuOpen = false;

    private FPController player;

    [SerializeField] private GameObject pauseMenu;

    private Animator PMAnimator;
    private bool animActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pauseMenu != null)
        {
            PMAnimator = pauseMenu.GetComponent<Animator>();
            player = FindFirstObjectByType<FPController>();



            SetMouseActive(false);
            SetPause(false);
            pauseMenu.SetActive(false);
        }
    

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetMouseActive(bool active)
    {
        if (active)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SetPause(bool pause)
    {
        if (pause)
        {
            paused = true;

            if (player != null)
            {
                player.MovementEnabled = false;
                player.CameraEnabled = false;
                player.LookEnabled = false;
            }
            

            Time.timeScale = 0;
        }

        else
        {
            paused = false;

            if (player != null)
            {
                player.MovementEnabled = true;
                player.CameraEnabled = true;
                player.LookEnabled = true;
            }
            

            Time.timeScale = 1;
        }
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void SetPauseMenu(bool pause) 
    {
        if (animActive) return;
        animActive = true;

        StartCoroutine(PauseAnim(pause));
    }

    private IEnumerator PauseAnim(bool active)
    {
        pauseMenu.SetActive(true);
        PMAnimator.SetBool("Active", active);
        SetMouseActive(active);
        SetPause(active);

        float animationLength = .55f; // Get actual animation length
        float elapsedTime = 0f;

        while (elapsedTime < animationLength)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }


        SetMouseActive(active);
        SetPause(active);       
        pauseMenu.SetActive(active);

        animActive = false;
        
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

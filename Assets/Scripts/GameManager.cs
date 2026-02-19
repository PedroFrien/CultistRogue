using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool paused = false;
    public bool menuOpen = false;

    [SerializeField] private GameObject pauseMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pauseMenu != null)
        {
            SetPauseMenu(false);
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
            Time.timeScale = 0;
        }

        else
        {
            paused = false;
            Time.timeScale = 1;
        }
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void SetPauseMenu(bool pause) 
    {
       if (pause)
        {
            SetMouseActive(true);
            SetPause(true);
            pauseMenu.SetActive(true);
        }
        else
        {
            SetMouseActive(false);
            SetPause(false);
            pauseMenu.SetActive(false);
        }
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

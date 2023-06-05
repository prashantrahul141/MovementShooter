using UnityEngine;
using UnityEngine.SceneManagement;

public class HudCanvas : MonoBehaviour
{
    public bool gameIsPaused = false;

    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    private GameObject resumeMenuUI;

    public Logger consoleLogger;

    private void Start()
    {
        consoleLogger = Component.FindAnyObjectByType<Logger>();
        Resume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            consoleLogger.showConsole = false;
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameIsPaused = true;
        pauseMenuUI.SetActive(true);
        resumeMenuUI.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameIsPaused = false;
        pauseMenuUI.SetActive(false);
        resumeMenuUI.SetActive(true);
    }

    public void MainMenuButton()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}

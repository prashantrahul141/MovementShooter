using UnityEngine;
using UnityEngine.SceneManagement;

public class HudCanvas : MonoBehaviour
{
    public bool gameIsPaused = false;

    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    private GameObject resumeMenuUI;

    public DebugController debugController;

    private void Start()
    {
        debugController = Component.FindAnyObjectByType<DebugController>();
        Resume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            debugController.changeUIState(false);
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

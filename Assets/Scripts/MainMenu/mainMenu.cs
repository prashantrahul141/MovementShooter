using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu_mainMenuUI;

    [SerializeField]
    private GameObject mainMenu_optionsMenuUI;

    public void Start()
    {
        mainMenu_mainMenuUI.SetActive(true);
        mainMenu_optionsMenuUI.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OptionsMenu()
    {
        mainMenu_mainMenuUI.SetActive(false);
        mainMenu_optionsMenuUI.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenu_optionsMenuUI.SetActive(false);
        mainMenu_mainMenuUI.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game.");
        Application.Quit();
    }
}

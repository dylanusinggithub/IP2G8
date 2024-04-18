using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Variables")]
    private bool isPaused = false;
    public GameObject pauseMenu;

    public Button mainMenu;
    public Button resumeButton;
    public Button quitButton;

    void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
        mainMenu.onClick.AddListener(MenuButton);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isPaused);
        }
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }

    public void ResumeGame()
    {
        TogglePause();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

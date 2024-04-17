using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("GameOver Variables")]
    public Button menuButton;
    public Button quitButton;

    private void OnEnable()
    {
        Time.timeScale = 0;
    }
    void Start()
    {
        menuButton.onClick.AddListener(MenuButton);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {

    }

    public void MenuButton()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
        Time.timeScale = 1;
    }
}

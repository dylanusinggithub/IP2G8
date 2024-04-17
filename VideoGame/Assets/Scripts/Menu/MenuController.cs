using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Menu Game Objects")]
    public Button startGameButton;
    public Button optionsButton;
    public Button exitButton;

    public GameObject settingsMenu;

    void Start()
    {
        //Add listeners
        startGameButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(ShowOptions);
        exitButton.onClick.AddListener(ExitGame);
    }

    public void StartGame()
    {
        //Start Game
        SceneManager.LoadScene("JackTest");
    }

    public void ShowOptions()
    {
        settingsMenu.SetActive(true);
    }

    public void ExitGame()
    {
        //Quit the game
        Application.Quit();
    }
}

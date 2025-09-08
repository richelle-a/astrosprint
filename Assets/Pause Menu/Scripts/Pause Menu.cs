//importing libraries
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //game objects and buttons for the menu
    public GameObject pauseMenuUI;
    public GameObject tutorialImg;
    public Button resumeButton;             
    public Button exitToPlayMenuButton;     
    public Button tutorialButton;   
    public Button pauseButton;

    void Start()
    {
        // Set the pause menu panel to inactive at start
        pauseMenuUI.SetActive(false);
    }

    public void OnPauseButtonClick()
    {
        if (pauseMenuUI.activeSelf)
        {
            Resume(); // resume the game
        }
        else
        {
            Pause(); // pause the game
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; //pause gameplay by freezing time
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // resume the game by unfreezing time
    }

    public void ShowTutorial()
    {
        tutorialImg.SetActive(true);
    }

    public void ExitToPlayMenu()
    {
        Time.timeScale = 1f; //makes time return to its normal progression
        SceneManager.LoadScene("Play Menu"); //loads the play menu scene
    }
}


//importing libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenuManager : MonoBehaviour
{
    //takes user to abilities menu
    public void GoToAbilities()
    {
        SceneManager.LoadScene("Abilities Menu");
    }

    //takes user to characters menu
    public void GoToCharacters() 
    {
        SceneManager.LoadScene("Character Menu");
    }

    //takes user to easy mode gameplay
    public void GoToEasyMode()
    {
        SceneManager.LoadScene("Easy Mode");
    }

    // takes user to tutorial scene
    public void GoToTutorial()
    {
        SceneManager.LoadScene("Tutorial Screen");
    }

    //allows user to use back button to go back to main menu
    public void GoBack()
    {
        SceneManager.LoadScene("Main Menu");
    }

}

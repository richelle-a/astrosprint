using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GoToLogin() // the subroutine attatched to the 'On Click' of the login button
    {
        //Accesses the build settings to load the Login Menu 
        SceneManager.LoadScene("Login Menu");
    }

    public void GoToSignUp() // the subroutine attatched to the 'On Click' of the sign up button
    {
        // Accesses the build settings to load the Login Menu 
        SceneManager.LoadScene("Sign Up Menu");
    }
}

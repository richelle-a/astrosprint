//importing libraries to use different functions in my game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginMenuManager : MonoBehaviour
{
    // initialises the UI elements for user inputs (username & password), feedback & login submit button
    public TMP_InputField usernameInput; 
    public TMP_InputField passwordInput; 
    public Button loginSubmitButton;    // Button for submitting the login request
    public TMP_Text feedbackText;       // text field to display feedback messages

    private string playFabTitleId = "45CD5"; // The PlayFab Title ID for AstroSprint, establishing the connection to the external database

    private void Start()
    {
        // Set the PlayFab Title ID for the current session.
        PlayFabSettings.staticSettings.TitleId = playFabTitleId;

        // Add a listener, something that waits for the button to be clicked, and then calls the login method onclick
        loginSubmitButton.onClick.AddListener(Login);
    }

    // handles the login process after the button is clicked, including validation and PlayFab API calls
    private void Login()
    {
        // Check if the username field is empty and alerts user to enter username if it is
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            feedbackText.text = "Please enter a username";
            return;
        }

        // Check if the password field is empty and alerts user to enter a password if it is
        if (string.IsNullOrEmpty(passwordInput.text))
        {
            feedbackText.text = "Please enter a password";
            return;
        }

        // creates a PlayFab login request for the inputted username & password
        var request = new LoginWithPlayFabRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text
        };

        // calls PlayFab's login API and handle the success or failure of the login
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }

    // suboroutine for successful login
    private void OnLoginSuccess(LoginResult result)
    {
        // Display a success message with the username.
        feedbackText.text = "Login successful! Welcome " + usernameInput.text;

        // Loads the Play Menu after login
        SceneManager.LoadScene("Play Menu");
    }

    // subroutine for login failure
    private void OnLoginFailure(PlayFabError error)
    {
        // displays an error message with details from PlayFab
        feedbackText.text = "Login failed: " + error.ErrorMessage;
    }

    //allows the user to go back to the main menu
    public void GoBack()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

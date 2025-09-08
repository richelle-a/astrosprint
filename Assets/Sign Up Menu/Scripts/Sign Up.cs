// importing libraries to use in the code
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.SceneManagement;

//the class that allows the user to sign up
public class SignUp : MonoBehaviour
{
    //creates input fields for all the different sign up details
    public TMP_InputField emailInput;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_Text feedbackText; // A UI text element for feedback messages

    public void OnSignUpButtonClicked()
    {
        // allowing the user to input all of the different sign up fields
        string newEmail = emailInput.text;
        string newUsername = usernameInput.text;
        string newPassword = passwordInput.text;
        string pass2Confirm = confirmPasswordInput.text;

        // checks if there is an input in the email field
        if (string.IsNullOrEmpty(newEmail))
        {
            feedbackText.text = "Please enter an email";
            return;
        }
        
        // checks if there is an input in the username field
         if (string.IsNullOrEmpty(newUsername))
        {
            feedbackText.text = "Please enter a username";
            return;
        }

        // checks if there is an input in the password field
        if (string.IsNullOrEmpty(newPassword))
        {
            feedbackText.text = "Please enter a password";
            return;
        }

        //Action taken based on testing: Checks if there is an input in the 'confirm password' field
        if (string.IsNullOrEmpty(pass2Confirm))
        {
            feedbackText.text = "Please enter a confirm password";
            return;
        }

        // checks if the password is between 8 and 16 characters
        if (newPassword.Length < 8 || newPassword.Length > 16)
        {
            feedbackText.text = "Ensure password is between 8 and 16 characters";
            return; 
        }

        // checks if the value entered in the confirm password field is the same as the password field
        if (newPassword != pass2Confirm)
        {
            feedbackText.text = "Please ensure that passwords match.";
            return;
        }

        // uses the email format subroutine to ensure email is in the correct format
        if (!IsValidEmail(newEmail))
        {
            ShowFeedback("Invalid email format. Please enter an email in the format 'someone@example.com'");
            emailInput.text = ""; // clears the email field
            return;
        }

        // uses the alphanumeric subroutine to ensure username only contains alpha numeric characters
        if (!IsAlphanumeric(newUsername))
        {
            ShowFeedback("Username can only contain alphanumeric characters."); //alerts the user of their error
            usernameInput.text = ""; // clears the username field
            return;
            
        }

        // Create a PlayFab registration request
        var registerRequest = new RegisterPlayFabUserRequest
        {
            Email = newEmail,
            Username = newUsername,
            Password = newPassword,
            RequireBothUsernameAndEmail = true
        };

        // Send the registration request to PlayFab
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    private bool IsValidEmail(string email)
    {
        // Using the regex pattern for emails to create a subroutinr for basic email validation
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }
    
    private bool IsAlphanumeric(string username)
    {
        // Using the regex pattern for emails to create a subroutinr for basic email validation
        string alphanumericPattern = @"^[a-zA-Z0-9]+$";
        return Regex.IsMatch(username, alphanumericPattern);
    }

    public void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        //tells the user that their registration was successful
        feedbackText.text = "Registration successful!";
        SceneManager.LoadScene("Play Menu");
        
    }

    public void OnRegisterFailure(PlayFabError error)
    {
        // tells the user that there's been an error
        feedbackText.text = "Error: " + error.ErrorMessage;
    }

    public void ShowFeedback(string message)
    {
        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);
    }

    // subrotuine attached to the onclick function of the back button
    public void GoBack()
    {
        //uses unity scene manager to load the main menu scene
        SceneManager.LoadScene("Main Menu");
    }
}
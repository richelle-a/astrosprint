using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class CharacterMenuManager : MonoBehaviour
{
    public Image playerIconImage; // image to show user what their selected icon is
    public Sprite maleAstronautIcon; // sprite image for male astronaut icon
    public Sprite femaleAstronautIcon; // sprite image for female astronaut icon
    public Sprite starIcon; // image sprite for star icon
    public Sprite alienIcon; // image sprite for alien icon

    private void Start()
    {
        // retrieve the previously selected character from PlayFab
        FetchCharacterFromPlayFab();
    }

    private void FetchCharacterFromPlayFab()
    {
        // retrieves character selection from PlayFab, and calls subroutines based on the result
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    private void OnDataReceived(GetUserDataResult result)
    {
        // retrieves the character name from PlayFab (if it is there)
        string selectedCharacter = "MaleAstronaut"; // defaults to male astronaut character if none is selected
        
        // checks that the "SelectedCharacter" key is present in PlayFab before attempting to retrieve data from it
        if (result.Data != null && result.Data.ContainsKey("SelectedCharacter"))
        {
            selectedCharacter = result.Data["SelectedCharacter"].Value;
        }

        // update the player icon image ased on the selected character
        UpdatePlayerIcon(selectedCharacter);
    }

    private void OnError(PlayFabError error)
    {
        // debug log of any errors
        Debug.LogError("Error fetching data from PlayFab: " + error.GenerateErrorReport()); 
        UpdatePlayerIcon("MaleAstronaut"); // Defaults to MaleAstronaut if there's an error
    }

    private void UpdatePlayerIcon(string characterName)
    {
        switch (characterName)
        {
            // changes the image of the player icon image depending on what character name is retrieved
            case "MaleAstronaut":
                playerIconImage.sprite = maleAstronautIcon;
                break;
            case "FemaleAstronaut":
                playerIconImage.sprite = femaleAstronautIcon;
                break;
            case "Star":
                playerIconImage.sprite = starIcon;
                break;
            case "Alien":
                playerIconImage.sprite = alienIcon;
                break;
            default:
                // debug log for any errors
                Debug.LogWarning("Unknown character selected: " + characterName);
                break;
        }
    }

    public void OnIconSelected(string characterName)
    {
        // saves the selected character to PlayFab
        SaveCharacterToPlayFab(characterName);

        // updates the player icon based on retrieved 
        UpdatePlayerIcon(characterName);
    }

    // creates a new PlayFab request to save new character selections
    private void SaveCharacterToPlayFab(string characterName)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "SelectedCharacter", characterName }
            }
        };

        // Save the selected character to PlayFab
        PlayFabClientAPI.UpdateUserData(request, OnDataSaved, OnError);
    }

    private void OnDataSaved(UpdateUserDataResult result)
    {
        Debug.Log("Character selection saved to PlayFab.");
    }

    public void GoBack()
    {
        // Go back to the play menu
        SceneManager.LoadScene("Play Menu");
    }
}

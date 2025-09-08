// importing libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class GameSummaryManager : MonoBehaviour
{

    //exit game summary to go to play menu
   public void ExitToPlayMenu()
    {
        SceneManager.LoadScene("Play Menu");
    }

    //text elements to show final score & final coins onscreen
    public TMP_Text finalCoinsText; 
    public TMP_Text finalScoreText;
    
    private int finalCoins; 
    public float finalScore;

    void Start()
    {
        FetchCurrentCoinsCollected();
        FetchCurrentScore();
    }

    //retrieves 'currentCoinsCollected' from PlayFab and saves it to finalCoins
    void FetchCurrentCoinsCollected()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("currentCoinsCollected"))
            {
                finalCoins = int.Parse(result.Data["currentCoinsCollected"].Value);
                DisplayFinalCoins();
            }
            else
            {
                Debug.LogError("CurrentCoinsCollected data not found in PlayFab.");
            }
        }, OnError);
    }

    void DisplayFinalCoins()
    {
        //displays the final coins collected on the summary
        finalCoinsText.text = finalCoins + "";
    }


    //retrives the current score from PlayFab & saves it to 'finalScore' 
    void FetchCurrentScore()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("currentScore"))
            {
                finalScore = float.Parse(result.Data["currentScore"].Value);
                DisplayFinalScore();
            }
            else
            {
                Debug.LogError("CurrentScore data not found in PlayFab.");
            }
        }, OnError);
    }

    void DisplayFinalScore()
    {
        // Display the final score onscreen
        finalScoreText.text = finalScore + "";
    }


    void OnError(PlayFabError error)
    {
        Debug.LogError("Error retrieving current coins collected from PlayFab: " + error.GenerateErrorReport());
    }
    
}

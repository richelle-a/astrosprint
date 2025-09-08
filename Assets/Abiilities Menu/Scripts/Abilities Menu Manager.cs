//import libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

public class AbilitiesMenuManager : MonoBehaviour
{
    public TMP_Text coinBalanceText; // text element to show coin balance
    public Button[] upgradeButtons; // creating an array for upgrade buttons for each power-up
    public GameObject[] doubleJumpBars, doubleCoinsBars, doubleScoreBars, flyingBars; // the actual bars for each power up
    public TMP_Text[] priceTexts; // text elements to display prices for each power up upgrade
    public Button backButton; // back button to go to play menu

    private int coinBalance = 0; 
    private int[] powerUpLevels = { 1, 1, 1, 1 }; // setting default power up levels to be 1
    private int[] prices = { 50, 70, 100, 200 }; // price for each upgrade level
    private float[] durations = { 30f, 40f, 50f, 60f, 120f }; // duration of each power up after levelling up
    private const string coinBalanceKey = "CoinBalance"; //field name in playfab
    private const string powerUpKey = "PowerUpLevels"; // field name in playfab
    public TMP_Text feedbackText; // text element for feedback messages

    private void Start()
    {
        //retrieving using info from playfab when menu is opened
        RetrieveCoinBalance(); 
        RetrievePowerUpLevels(); 
        UpdateUI(); // updates menu UI based on info retrieved
    }


    // subroutine for back button
    public void GoBack()
    {
        SceneManager.LoadScene("Play Menu");
    }

    // retrieves the user's coin balance from PlayFab
    public void RetrieveCoinBalance()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnGetUserDataSuccess, OnPlayFabError);
    }


    //subroutine for sucessful data retrieval
    private void OnGetUserDataSuccess(GetUserDataResult result)
    {
        // if coin balance is found then set it to variable declared earlier
        if (result.Data != null && result.Data.ContainsKey(coinBalanceKey))
        {
            coinBalance = int.Parse(result.Data[coinBalanceKey].Value);
        }
        else
        {
            coinBalance = 0; // set to default if no coin balance found
            SaveCoinBalance();
        }
        UpdateCoinBalanceText();
        UpdateUI(); 
    }

    // alerts user of any errors
    private void OnPlayFabError(PlayFabError error)
    {
        Debug.LogError("Error retrieving data from PlayFab: " + error.GenerateErrorReport());
    }

    // saves player's coin balance to PlayFab
    public void SaveCoinBalance()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> { { coinBalanceKey, coinBalance.ToString() } }
        };
        PlayFabClientAPI.UpdateUserData(request, OnSaveUserDataSuccess, OnPlayFabError);
    }

    private void OnSaveUserDataSuccess(UpdateUserDataResult result)
    {
        UpdateCoinBalanceText();
        Debug.Log("Coin balance successfully updated in PlayFab.");
    }

    // retrieves power up levels from PlayFab
    public void RetrievePowerUpLevels()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnGetPowerUpLevelsSuccess, OnPlayFabError);
    }

    private void OnGetPowerUpLevelsSuccess(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey(powerUpKey))
        {
            string[] levels = result.Data[powerUpKey].Value.Split(',');
            for (int i = 0; i < levels.Length; i++)
            {
                powerUpLevels[i] = int.Parse(levels[i]);
            }
        }
        else
        {
            powerUpLevels = new int[] { 1, 1, 1, 1 }; // sets levels to default (1) if no data found
            SavePowerUpLevels(); // saves the default values
        }
        UpdateUI(); // updates UI after retrieving levels
    }

    // saves power up levels to PlayFab
    public void SavePowerUpLevels()
    {
        string levels = string.Join(",", powerUpLevels);
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> { { powerUpKey, levels } }
        };
        PlayFabClientAPI.UpdateUserData(request, OnSaveUserDataSuccess, OnPlayFabError);
    }

    // updates the text element to display current coin balance
    private void UpdateCoinBalanceText()
    {
        coinBalanceText.text = coinBalance.ToString();
    }

    // updates the bars & price texts for each power up
    private void UpdateUI()
    {
        UpdatePowerUpUI(doubleJumpBars, 0, priceTexts[0]);
        UpdatePowerUpUI(doubleCoinsBars, 1, priceTexts[1]);
        UpdatePowerUpUI(doubleScoreBars, 2, priceTexts[2]);
        UpdatePowerUpUI(flyingBars, 3, priceTexts[3]);
    }

    // updates the bars and price text for a specific power up
    private void UpdatePowerUpUI(GameObject[] bars, int powerUpIndex, TMP_Text priceText)
    {
        if (bars == null || priceText == null || upgradeButtons.Length <= powerUpIndex)
        {
            Debug.LogWarning("Missing UI elements for power-up index: " + powerUpIndex);
            return;
        }

        // initially hides all the bars
        foreach (var bar in bars)
        {
            bar.SetActive(false);
        }

        // displays the amount bars based on the power up level
        for (int i = 0; i < powerUpLevels[powerUpIndex]; i++)
        {
            bars[i].SetActive(true);
        }

        // updates the price text for the next upgrade
        if (powerUpLevels[powerUpIndex] < prices.Length)
        {
            priceText.text = prices[powerUpLevels[powerUpIndex]].ToString(); 
        }
        else
        {
            priceText.text = "MAX"; // if max level, disable further upgrades
            upgradeButtons[powerUpIndex].interactable = false; // disable button when max level reached
            feedbackText.text = "You have reached the maximum power up level";
        }
    }

    // subroutine to update a specific power up
    public void TryUpgradePowerUp(int powerUpIndex)
    {
        if (powerUpLevels[powerUpIndex] < prices.Length && coinBalance >= prices[powerUpLevels[powerUpIndex]])
        {
            // deducts the price of the upgrade from balance
            coinBalance -= prices[powerUpLevels[powerUpIndex]];
            UpdateCoinBalanceText();

            // updates & displays the power up level & saves the new coin balance & power up levels
            powerUpLevels[powerUpIndex]++;
            SaveCoinBalance();
            SavePowerUpLevels();
            UpdateUI();
        }
        else
        {
            feedbackText.text = "You don't have enough coins for this upgrade";

        }
    }
}


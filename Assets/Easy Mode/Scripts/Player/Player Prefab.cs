// importing libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerPrefab : MonoBehaviour
{
    public float jumpForce = 10f; // the force the player jumps with
    private bool isGrounded; // whether the player is on the ground or not
    private Rigidbody2D rb; // Rigidbody2D to apply force
    private SpriteRenderer sr; // SpriteRenderer for character sprite
    public PlayerController playerController; //accesses play manager script


    public TMP_Text coinsCollectedText; //text element to display coins collected
    public TMP_Text powerUpText; // text element to display active power up 

    // variables for each character icon sprite
    public Sprite maleAstronautIcon;
    public Sprite femaleAstronautIcon;
    public Sprite starIcon;
    public Sprite alienIcon;

    public int coinsCollected = 0; //number of coins collected by the player

    // tracking power ups
    private int doubleJumpCount = 0;
    private bool doubleCoinsActive = false;
    private bool flyingActive = false;
    public bool twoScoreActive = false;

    private float flyingMaxHeight;

    private Dictionary<string, float> powerUpDurations = new Dictionary<string, float>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        flyingMaxHeight = Camera.main.orthographicSize * 2; // ,ax height based on camera size

        // retrieves the character sprite from PlayFab
        FetchCharacterIcon();
    }

    void Update()
    {
        // handles jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            //jumping
            rb.velocity = Vector2.up * jumpForce;
            isGrounded = false;
            doubleJumpCount = 1; // resets double jump count
        }
        else if (!isGrounded && doubleJumpCount < 2 && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.up * jumpForce;
            doubleJumpCount++;
        }

        // handles flying
        if (flyingActive && Input.GetKey(KeyCode.Space) && transform.position.y < flyingMaxHeight)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // checks if the player is outside the camera bounds
        if (IsPlayerOutOfCameraBounds())
        {
            PlayerDeath();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if the player is on the ground, allow them to jump jumping
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // coin collection
        if (collision.gameObject.CompareTag("Coin"))
        {
            coinsCollected += doubleCoinsActive ? 2 : 1; // double coins if power up active
            Debug.Log("Coins Collected: " + coinsCollected);
            coinsCollectedText.text = "Coins: " + coinsCollected;

            Destroy(collision.gameObject); // delete coin after collection
        }

        // handles collision with power ups
        if (collision.gameObject.CompareTag("Double Jump"))
        {
            ActivatePowerUp("Double Jump");
        }
        else if (collision.gameObject.CompareTag("Double Coins"))
        {
            ActivatePowerUp("Double Coins");
        }
        else if (collision.gameObject.CompareTag("Double Score"))
        {
            ActivatePowerUp("Double Score");
        }
        else if (collision.gameObject.CompareTag("Flying"))
        {
            ActivatePowerUp("Flying");
        }

        // kill player if they collide with obstacles
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            PlayerDeath();
        }
    }

    private void ActivatePowerUp(string powerUpName)
    {
        powerUpText.text = powerUpName; // displays the name of the power-up


        // gets the duration of the power up
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("PowerUpLevels"))
            {
                string[] powerUpLevels = result.Data["PowerUpLevels"].Value.Split(',');
                int powerUpIndex = GetPowerUpIndex(powerUpName);

                if (powerUpIndex >= 0 && powerUpIndex < powerUpLevels.Length)
                {
                    int level = int.Parse(powerUpLevels[powerUpIndex]);
                    float duration = GetDurationForLevel(level);

                    powerUpDurations[powerUpName] = duration;

                    StartCoroutine(HandlePowerUpEffect(powerUpName, duration));
                }
            }
        }, OnError);
    }

    private IEnumerator HandlePowerUpEffect(string powerUpName, float duration)
    {
        if (powerUpName == "Double Jump")
        {
            doubleJumpCount = 0; // allow double jump
        }
        else if (powerUpName == "Double Coins")
        {
            doubleCoinsActive = true;
        }
        else if (powerUpName == "Double Score")
        {
            twoScoreActive = true;
        }
        else if (powerUpName == "Flying")
        {
            flyingActive = true;
        }

        yield return new WaitForSeconds(duration);

        // reset the effects after duration has passed
        if (powerUpName == "Double Jump")
        {
            doubleJumpCount = 0;
        }
        else if (powerUpName == "Double Coins")
        {
            doubleCoinsActive = false;
        }
        else if (powerUpName == "Double Score")
        {
            twoScoreActive = false;
        }
        else if (powerUpName == "Flying")
        {
            flyingActive = false;
        }

        powerUpText.text = ""; // clear power up name display
    }

    private int GetPowerUpIndex(string powerUpName)
    {
        switch (powerUpName)
        {
            case "Double Jump": return 0;
            case "Double Coins": return 1;
            case "Double Score": return 2;
            case "Flying": return 3;
            default: return -1;
        }
    }

    private float GetDurationForLevel(int level)
    {
        float[] durations = { 30f, 40f, 50f, 60f, 120f }; // level-based durations
        return level > 0 && level <= durations.Length ? durations[level - 1] : 30f;
    }

    private void FetchCharacterIcon()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    private void OnDataReceived(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("SelectedCharacter"))
        {
            string characterName = result.Data["SelectedCharacter"].Value;
            SetCharacter(characterName);
        }
        else
        {
            Debug.LogWarning("SelectedCharacter key not found in PlayFab data. Defaulting to MaleAstronaut.");
            SetCharacter("MaleAstronaut");
        }
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error fetching data from PlayFab: " + error.GenerateErrorReport());
        SetCharacter("MaleAstronaut");
    }

    // sets player character sprite
    private void SetCharacter(string characterName)
    {
        switch (characterName)
        {
            case "MaleAstronaut":
                sr.sprite = maleAstronautIcon;
                break;
            case "FemaleAstronaut":
                sr.sprite = femaleAstronautIcon;
                break;
            case "Star":
                sr.sprite = starIcon;
                break;
            case "Alien":
                sr.sprite = alienIcon;
                break;
            default:
                sr.sprite = maleAstronautIcon;
                break;
        }
    }

    private bool IsPlayerOutOfCameraBounds()
    {
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPos.x < 0 || playerScreenPos.x > Screen.width || playerScreenPos.y < 0 || playerScreenPos.y > Screen.height;
    }


    // subroutine for when player dies
    private void PlayerDeath()
    {
        SaveCoinsToPlayFab();
        SaveCurrentScoreToPlayFab(playerController.currentScore);
        Destroy(gameObject);
        SceneManager.LoadScene("Game Summary");
    }

    void SaveCoinsToPlayFab()
    {
        // save the coins collected to the 'currentCoinsCollected' in playfab
        var data = new Dictionary<string, string>
        {
            { "currentCoinsCollected", coinsCollected.ToString() }
        };

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = data
        }, result =>
        {
            // retrieves the current coin balance from PlayFab
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
            {
                if (result.Data != null && result.Data.ContainsKey("CoinBalance"))
                {
                    int currentCoinBalance = int.Parse(result.Data["CoinBalance"].Value);
                    int updatedCoinBalance = currentCoinBalance + coinsCollected;

                    // update the 'CoinBalance' field with the new value
                    var updatedData = new Dictionary<string, string>
                    {
                        { "CoinBalance", updatedCoinBalance.ToString() }
                    };

                    PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
                    {
                        Data = updatedData
                    }, OnDataSaved, OnError);
                }
                else
                {
                    Debug.LogError("CoinBalance data not found in PlayFab.");
                }
            }, OnError);
        }, OnError);
    }

    private void OnDataSaved(UpdateUserDataResult result)
    {
        Debug.Log("Coin balance updated successfully in PlayFab.");
    }

    void SaveCurrentScoreToPlayFab(float scoreCurrent)
    {
        // prepares data to be updated on PlayFab
        var data = new Dictionary<string, string>
        {
            { "currentScore", scoreCurrent.ToString() }
        };

       // updates the user data on PlayFab
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
       {
            Data = data
       }, WhenDataSaved, OnError);
    }

    private void WhenDataSaved(UpdateUserDataResult result)
    {   
        Debug.Log("Current score saved successfully to PlayFab.");
    }

}

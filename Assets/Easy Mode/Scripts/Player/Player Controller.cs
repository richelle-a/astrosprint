//importing libraries
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public TMP_Text scoreText;
    public float currentScore = 0f;
    public float baseScore = 10f;
    public float timer = 0f;
    public float addScoreInterval = 1f; // time interval in seconds to add score

    private float nextScoreTime = 0f; // time when score should next increase

    // declared to able to interact with player script
    [SerializeField] private PlayerPrefab playerPrefabScript; 

    void Start()
    {
        nextScoreTime = addScoreInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Add score at regular intervals
        if (timer >= nextScoreTime)
        {
            if (playerPrefabScript != null && playerPrefabScript.twoScoreActive)
            {
                currentScore += baseScore * 2; // Double the score added if the double score is true
            }
            else
            {
                currentScore += baseScore; // regular score addition
            }

            UpdateScoreUI();
            nextScoreTime += addScoreInterval; // schedules the next score increment
        }
    }

    // shows score onscreen
    public void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.RoundToInt(currentScore);
        }
    }
}

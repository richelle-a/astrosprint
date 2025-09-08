using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    private string previousSceneName;   // Stores the last scene name
    public string currentSceneName;     // Current scene name

    void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure persistence across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name; // Track the current scene name
    }

    private void Update()
    {
        currentSceneName = SceneManager.GetActiveScene().name; // Track the current scene name

    }

    // Load a new scene and save the current scene name
    public void LoadScene(string sceneName)
    {
        previousSceneName = SceneManager.GetActiveScene().name; // Save the current scene name
        SceneManager.LoadScene(sceneName);                     // Load the new scene
    }

    // Return to the previously saved scene
    public void LoadPreviousScene()
    {
        if (!string.IsNullOrEmpty(previousSceneName))
        {
            SceneManager.LoadScene(previousSceneName); // Load the previous scene
        }
        else
        {
            Debug.LogWarning("No previous scene recorded!");
        }
    }
}
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;     //declaring coin prefab to later instantiate

    [SerializeField]
    public float spawnInterval = 5f;  //interval between coin spawns
    public float spawnHeight = 10f;   //height that coins spawn at

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;  //get the main camera

        //start spawning coins at regular intervals
        InvokeRepeating("SpawnCoin", 0f, spawnInterval);
    }

    void SpawnCoin()
    {
        //get the camera's size in world units
        float cameraWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect;
        float cameraHeight = mainCamera.orthographicSize * 2f;

        //generate a random position for the coin within the camera's bounds
        float randomX = Random.Range(-cameraWidth / 2, cameraWidth / 2);  //generate random x-coordinate
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0f);  

        //instantiate the coin at the random position
        Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
    }
}


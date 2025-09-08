//importing libraries
using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; //prefab to spawn
    public float spawnInterval = 10f; //time interval between spawn
    public float yPosition = -0.46f; //y position for obstacles spawning

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnObstacles());
    }

    //subroutine to spawn obstacles periodically by calling subroutine periodically
    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            SpawnObstacle();
            yield return new WaitForSeconds(spawnInterval);
        }
    }


    void SpawnObstacle()
    {
        if (obstaclePrefab == null || mainCamera == null) return;

        // gets bounds of the main camera
        float minX = mainCamera.ViewportToWorldPoint(new Vector3(0.8f, 0, 0)).x; //centre of the screen
        float maxX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x; //right side of the screen

        //generate a random x-position
        float randomX = Random.Range(minX, maxX);

        //set the spawn position
        Vector3 spawnPosition = new Vector3(randomX, yPosition, 0);

        //instantiate the obstacle
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }
}




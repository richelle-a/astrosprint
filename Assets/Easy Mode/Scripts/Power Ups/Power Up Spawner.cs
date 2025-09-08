using System.Collections;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs; //array of power up prefabs
    public float spawnInterval = 14f; //interval between spawns
    public float yPosition = 5f; //y position for spawning power ups
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnPowerUps());
    }

    //subroutine to spawn power ups periodically
    IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            SpawnPowerUp();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnPowerUp()
    {
        if (powerUpPrefabs.Length == 0 || mainCamera == null) return;

        //get the bounds of the main camera
        float minX = mainCamera.ViewportToWorldPoint(new Vector3(0.7f, 0, 0)).x; //centre of the screen
        float maxX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;   //right side of the screen

        //generate random x-position
        float randomX = Random.Range(minX, maxX);

        //set spawn position
        Vector3 spawnPosition = new Vector3(randomX, yPosition, 0);

        //chooses a random prefab from the array
        int randomIndex = Random.Range(0, powerUpPrefabs.Length);
        GameObject selectedPowerUp = powerUpPrefabs[randomIndex];

        //instantiate the power-up
        Instantiate(selectedPowerUp, spawnPosition, Quaternion.identity);
    }
}





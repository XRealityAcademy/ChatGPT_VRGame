using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    public GameObject flowerPrefab; // Reference to the flower prefab
    public float spawnInterval = 2f; // Interval between each flower spawn
    public float spawnRadius = 2f; // Radius around the spawner to spawn the flowers

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFlowers());
    }

    IEnumerator SpawnFlowers()
    {
        while (true)
        {
            // Spawn the flower prefab at a random position within the spawn radius
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = -0.5f; // Ensure the flower is spawned at floor level
            Instantiate(flowerPrefab, spawnPosition, Quaternion.identity);

            // Wait for the next spawn interval
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

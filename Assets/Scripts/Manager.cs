using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject bullet;
    public GameObject player;
    public GameObject bat;
    public float batSpawnInterval = 2f;
    public float batSpawnRadius = 5f;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }

    void Start()
    {
        StartCoroutine(SpawnBats());
    }

    IEnumerator SpawnBats()
    {
        while (true)
        {
            // Spawn the flower prefab at a random position within the spawn radius
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * batSpawnRadius;
            spawnPosition.y = 0.01f; // Ensure the flower is spawned at floor level
            Instantiate(bat, spawnPosition, Quaternion.identity);

            // Wait for the next spawn interval
            yield return new WaitForSeconds(batSpawnInterval);
        }
    }
}

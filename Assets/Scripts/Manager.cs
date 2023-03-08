using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject bullet;
    public GameObject player;
   // public GameObject Gnome;
    public GameObject Bat;
    public GameObject Gnome;
    public float batSpawnInterval = 2f;
    public float batSpawnRadius = 5f;
    public float gnomeSpawnInterval = 4f;
    public float gnomeSpawnRadius = 3f;
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
            spawnPosition.y = 0.5f; // Ensure the flower is spawned at floor level
           // Instantiate(Gnome, spawnPosition, Quaternion.identity);
            Instantiate(Bat, spawnPosition, Quaternion.identity);

            // Wait for the next spawn interval
            yield return new WaitForSeconds(batSpawnInterval);
        }
    }

    IEnumerator SpawnGnome()
    {
        while (true)
        {
            // Spawn the flower prefab at a random position within the spawn radius
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * gnomeSpawnRadius;
            spawnPosition.y = 0; // Ensure the flower is spawned at floor level
                                    // Instantiate(Gnome, spawnPosition, Quaternion.identity);
            Instantiate(Gnome, spawnPosition, Quaternion.identity);

            // Wait for the next spawn interval
            yield return new WaitForSeconds(gnomeSpawnInterval);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawnManager : MonoBehaviour
{
    public List<SpawnData> SpawnOrder;

    [SerializeField] GameObject player;
    private int enemiesAliveCount = 0;
    // How spread apart the enemies can be
    private float spawnSpread = 3f;
    // Minimum distance from all existing enemies
    private float minimumSeparationDistance = 1.5f;
    [System.Serializable]
    public class SpawnData
    {
        public GameObject prefab;
        public GameObject spawnLocation;
        public int count;
        public bool requireAllDead = true;
    }

    void Update()
    {
        enemiesAliveCount = GameObject.FindGameObjectsWithTag("Enemy")
            .Count(enemy => enemy.GetComponent<Health>()?.CurrentHealth > 0);

        if (SpawnOrder.Count > 0)
            SpawnNextEnemies();
    }

    private void SpawnNextEnemies()
    {
        SpawnData nextSpawnData = SpawnOrder.FirstOrDefault();

        // Spawn without checking enemies alive if requireAllDead is false, else do the check
        if (nextSpawnData != null && (!nextSpawnData.requireAllDead || enemiesAliveCount <= 0))
        {
            Vector3 spawnPosition = nextSpawnData.spawnLocation.transform.position;

            for (int i = 0; i < nextSpawnData.count; i++)
            {
                Vector3 randomizedPosition = GetRandomizedSpawnPosition(spawnPosition);

                // Ensure the new spawn position is not too close to an existing enemy
                while (IsPositionTooCloseToExisting(randomizedPosition))
                {
                    randomizedPosition = GetRandomizedSpawnPosition(spawnPosition);
                }

                float delay = i * 1.0f; // Adjust the delay time based on your preference
                InstantiateEnemy(nextSpawnData.prefab, randomizedPosition);
            }

            // Remove the spawn data, all enemies spawned
            SpawnOrder.Remove(nextSpawnData);
        }
    }

    private Vector3 GetRandomizedSpawnPosition(Vector3 basePosition)
    {
        float randomX = Random.Range(-spawnSpread, spawnSpread);
        float randomZ = Random.Range(-spawnSpread, spawnSpread);

        Vector3 randomizedPosition = basePosition + new Vector3(randomX, 0f, randomZ);

        return randomizedPosition;
    }

    private bool IsPositionTooCloseToExisting(Vector3 position)
    {
        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in existingEnemies)
        {
            float distance = Vector3.Distance(position, enemy.transform.position);
            if (distance < minimumSeparationDistance)
            {
                return true; // Too close to an existing enemy
            }
        }

        return false; // Acceptable separation
    }

    private void InstantiateEnemy(GameObject prefab, Vector3 position)
    {
        GameObject enemy = Instantiate(prefab, position, Quaternion.identity);
        enemy.layer = LayerMask.NameToLayer("Enemy");
    }

    public void AddEnemies(SpawnData spawnData)
    {
        SpawnOrder.Add(spawnData);
    }
}

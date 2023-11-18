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
            SpawnEnemies(nextSpawnData);

            // Remove the spawn data, all enemies spawned
            SpawnOrder.Remove(nextSpawnData);
        }
    }

    void SpawnEnemies(SpawnData spawnData)
    {
        int totalCount = spawnData.count;
        int rows = (int)Mathf.Ceil(Mathf.Sqrt(totalCount));
        int columns = (int)Mathf.Ceil((float)totalCount / rows);
        float spacing = spawnSpread;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int index = row * columns + col;

                // If the index is beyond the total count, break the loop
                if (index >= totalCount)
                    break;

                float xPos = col * spacing;
                float zPos = row * spacing;

                Vector3 offset = new Vector3(xPos, 0f, zPos);
                Vector3 desiredSpawnPosition = spawnData.spawnLocation.transform.position + offset;

                // Adjust the position to ensure it maintains a minimum separation distance
                Vector3 finalSpawnPosition = GetValidSpawnPosition(desiredSpawnPosition);

                GameObject enemy = Instantiate(spawnData.prefab, finalSpawnPosition, spawnData.spawnLocation.transform.rotation);
                enemy.layer = LayerMask.NameToLayer("Enemy");
            }
        }
    }
    Vector3 GetValidSpawnPosition(Vector3 desiredPosition)
    {
        // Check if the desired position is too close to any existing enemy
        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var existingEnemy in existingEnemies)
        {
            float distance = Vector3.Distance(desiredPosition, existingEnemy.transform.position);

            // If too close, adjust the position
            if (distance < minimumSeparationDistance)
            {
                // Move the desired position away from the existing enemy
                Vector3 directionToExisting = (desiredPosition - existingEnemy.transform.position).normalized;
                desiredPosition += directionToExisting * (minimumSeparationDistance - distance);
            }
        }

        return desiredPosition;
    }

    public void SpawnEnemiesNow(SpawnData spawnData)
    {
        SpawnOrder.Insert(0, spawnData);
    }
}

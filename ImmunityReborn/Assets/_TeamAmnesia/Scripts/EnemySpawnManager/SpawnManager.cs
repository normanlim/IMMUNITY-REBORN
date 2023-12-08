using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] public List<SpawnData> SpawnQueue;
    [SerializeField] GameObject player;
    private int enemiesAliveCount = 0;
    // How spread apart the enemies can be
    private float spawnSpread = 2f;
    // Minimum distance from all existing entities
    private float minimumSeparationDistance = 1f;
    [SerializeField] private int queueIndex;

    [System.Serializable]
    public class SpawnGroup
    {
        public GameObject prefab;
        public int count;
    }

    [System.Serializable]
    public class SpawnData
    {
        public List<SpawnGroup> spawnGroups;
        public GameObject spawnLocation;
        public bool requireAllDead = true;
    }

    private void OnEnable()
    {
        queueIndex = 0;
    }

    private void OnDisable()
    {
        queueIndex = -1;
    }

    void Update()
    {
        enemiesAliveCount = GameObject.FindGameObjectsWithTag("Enemy")
            .Count(enemy => enemy.GetComponent<Health>()?.CurrentHealth > 0);

        if (SpawnQueue.Count > 0)
            TrySpawnNext();
    }

    /* Check the following before spawning the next dataset:
     * (1) if the spawn manager object is active
     * (2) if the queue index is in bounds
     * (3) if requireAllDead is false, else if all enemies are dead
     */
    private void TrySpawnNext()
    {
        if (gameObject.activeSelf && queueIndex >= 0 && queueIndex < SpawnQueue.Count)
        {
            SpawnData nextSpawnData = SpawnQueue[queueIndex];

            if (nextSpawnData != null && (!nextSpawnData.requireAllDead || enemiesAliveCount <= 0))
            {
                SpawnDataset(nextSpawnData);
                queueIndex++;
            }
        }
    }

    /* Given the spawn data, spawn all groups of enemies at the spawn location
     */
    public void SpawnDataset(SpawnData spawnData)
    {
        float spacing = spawnSpread;

        // Calculate total count across all spawn groups
        int totalCount = spawnData.spawnGroups.Sum(group => group.count);

        // Calculate rows and columns for the total count
        int totalRows = (int)Mathf.Ceil(Mathf.Sqrt(totalCount));
        int totalColumns = (int)Mathf.Ceil((float)totalCount / totalRows);

        // Iterate through the total rows and columns
        for (int row = 0; row < totalRows; row++)
        {
            for (int col = 0; col < totalColumns; col++)
            {
                int index = row * totalColumns + col;

                // If the index is beyond the total count, break the loop
                if (index >= totalCount)
                    break;

                float xPos = col * spacing;
                float zPos = row * spacing;

                Vector3 offset = new Vector3(xPos, 0f, zPos);
                Vector3 desiredSpawnPosition = spawnData.spawnLocation.transform.position + offset;

                // Adjust the position to ensure it maintains a minimum separation distance
                Vector3 finalSpawnPosition = GetValidSpawnPosition(desiredSpawnPosition);

                // Find the appropriate spawn group based on the current index
                SpawnGroup spawnGroup = FindSpawnGroupAtIndex(spawnData.spawnGroups, index);

                if (spawnGroup != null)
                {
                    GameObject entity = Instantiate(spawnGroup.prefab, finalSpawnPosition, spawnData.spawnLocation.transform.rotation);
                    // entity parent is set to the spawn location
                    entity.transform.parent = spawnData.spawnLocation.transform;
                }
            }
        }
    }

    SpawnGroup FindSpawnGroupAtIndex(List<SpawnGroup> spawnGroups, int index)
    {
        int currentIndex = 0;

        foreach (var spawnGroup in spawnGroups)
        {
            int groupCount = spawnGroup.count;

            // Check if the index is within the current group
            if (index < currentIndex + groupCount)
            {
                return spawnGroup;
            }

            currentIndex += groupCount;
        }

        return null;
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
}

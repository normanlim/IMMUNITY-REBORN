using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    public List<SpawnData> MeleeSpawnLocations;
    public List<SpawnData> RangedSpawnLocations;
    public List<SpawnData> MagicSpawnLocations;
    public SpawnData bossSpawnLocation;
    [SerializeField] GameObject meleeEnemyPrefab;
    [SerializeField] GameObject rangedEnemyPrefab;
    [SerializeField] GameObject magicEnemyPrefab;
    [SerializeField] List<GameObject> minibossPrefabs;
    [SerializeField] GameObject finalBossPrefab;
    public int bossSpawnThreshold = 4;
    public int enemiesKilled = 0;
    private int minibossesKilled = 0;
    private GameObject activeBoss = null;

    [System.Serializable]
    public class SpawnData
    {
        public Vector3 position;
        public Vector3 rotation;
    }

    void Update()
    {
        SpawnEnemies(meleeEnemyPrefab, MeleeSpawnLocations);
        SpawnEnemies(rangedEnemyPrefab, RangedSpawnLocations);
        SpawnEnemies(magicEnemyPrefab, MagicSpawnLocations);
        SpawnNextMiniboss();
        SpawnBoss();

    }

    void SpawnEnemies(GameObject enemyPrefab, List<SpawnData> spawnLocations)
    {
        foreach (SpawnData spawnData in spawnLocations)
        {
            Vector3 position = spawnData.position;
            Quaternion rotationEulerAngles = Quaternion.Euler(spawnData.rotation);

            Instantiate(enemyPrefab, position, rotationEulerAngles);
        }
        // Clear the locations, all enemies spawned
        spawnLocations.Clear();

    }

    // Call this method to increase the minibossesKilled count
    public void MinibossKilled()
    {
        minibossesKilled++;
        // Reset enemies killed counter for next miniboss
        enemiesKilled = 0;
        // Set the active miniboss to null when a miniboss is defeated
        activeBoss = null;
    }

    void SpawnBoss()
    {
        // Check for final boss spawn when all minibosses are defeated
        if (activeBoss == null && minibossesKilled >= minibossPrefabs.Count)
        {
            activeBoss = Instantiate(finalBossPrefab, bossSpawnLocation.position, Quaternion.Euler(bossSpawnLocation.rotation));
        }
    }

    // Call this method to spawn the next miniboss
    public void SpawnNextMiniboss()
    {
        // Only spawn a new miniboss if there's no active miniboss
        if (enemiesKilled >= bossSpawnThreshold && activeBoss == null && minibossesKilled < minibossPrefabs.Count)
        {
            activeBoss = Instantiate(minibossPrefabs[minibossesKilled], bossSpawnLocation.position, Quaternion.Euler(bossSpawnLocation.rotation));
        }
    }

    public void AddMeleeEnemies(List<SpawnData> spawnDatas)
    {
        MeleeSpawnLocations.AddRange(spawnDatas);
    }

    public void AddRangedEnemies(List<SpawnData> spawnDatas)
    {
        RangedSpawnLocations.AddRange(spawnDatas);
    }

    public void AddMagicEnemies(List<SpawnData> spawnDatas)
    {
        MagicSpawnLocations.AddRange(spawnDatas);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    public List<SpawnData> MeleeSpawnLocations;
    public List<SpawnData> RangedSpawnLocations;
    public List<SpawnData> MagicSpawnLocations;
    [SerializeField] GameObject meleeEnemyPrefab;
    [SerializeField] GameObject rangedEnemyPrefab;
    [SerializeField] GameObject magicEnemyPrefab;
    // May want to keep track by type to spawn specific bosses
    public int enemiesKilled = 0;

    [System.Serializable]
    public class SpawnData
    {
        public Vector3 position;
        public Vector3 rotation;
    }

    void Start()
    {
        SpawnEnemies(meleeEnemyPrefab, MeleeSpawnLocations);
        SpawnEnemies(rangedEnemyPrefab, RangedSpawnLocations);
        SpawnEnemies(magicEnemyPrefab, MagicSpawnLocations);
    }

    void SpawnEnemies(GameObject enemyPrefab, List<SpawnData> spawnLocations)
    {
        foreach (SpawnData spawnData in spawnLocations)
        {
            // Get the position and rotation from the SpawnData
            Vector3 position = spawnData.position;
            // Convert Vector3 rotation to Quaternion
            Quaternion rotationEulerAngles = Quaternion.Euler(spawnData.rotation);

            GameObject enemy = Instantiate(enemyPrefab, position, rotationEulerAngles);
            Transform launchOrigin = enemy.transform.Find("ProjectileLaunchOrigin");
            if (launchOrigin != null)
            {
                // Assuming that the shoot projectile script is attached to the ProjectileLaunchOrigin
                EnemyShootProjectile shootProjectileScript = launchOrigin.GetComponent<EnemyShootProjectile>();
                if (shootProjectileScript != null)
                {
                    // Set the player reference in the script
                    shootProjectileScript.player = player;
                }
            }
        }
    }
}

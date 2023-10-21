using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject player;
    public List<Vector3> MeleeSpawnLocations;
    public List<Vector3> RangedSpawnLocations;
    public List<Vector3> MagicSpawnLocations;
    public GameObject meleeEnemyPrefab;
    public GameObject rangedEnemyPrefab;
    public GameObject magicEnemyPrefab;

    void Start()
    {
        SpawnEnemies(meleeEnemyPrefab, MeleeSpawnLocations);
        SpawnEnemies(rangedEnemyPrefab, RangedSpawnLocations);
        SpawnEnemies(magicEnemyPrefab, MagicSpawnLocations);
    }

    void SpawnEnemies(GameObject enemyPrefab, List<Vector3> spawnLocations)
    {
        foreach (Vector3 location in spawnLocations)
        {
            GameObject enemy = Instantiate(enemyPrefab, location, Quaternion.identity);
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

using System.Collections.Generic;
using UnityEngine;

public class MinibossDeath : MonoBehaviour
{
    public List<EnemySpawnManager.SpawnData> spawnData;

    public void initNextStage(EnemySpawnManager enemySpawnManager)
    {
        enemySpawnManager.MinibossKilled();
        enemySpawnManager.AddMeleeEnemies(spawnData);
    }
}

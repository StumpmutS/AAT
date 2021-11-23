using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    private static int defeatedSpawnerCount = 0;

    private void Awake()
    {
        EnemySpawnPlotManager.OnNextWave += ResetDefeatedSpawnerCount;
    }

    public static void SpawnerDefeatHandler()
    {
        defeatedSpawnerCount++;
        print("spawner: " + defeatedSpawnerCount);
        if (defeatedSpawnerCount >= EnemySpawnPlotManager.SpawnerPlots.Count)
        {
            print("send next wave");
            EnemySpawnPlotManager.NextWave();
        }
    }
    
    private void ResetDefeatedSpawnerCount()
    {
        defeatedSpawnerCount = 0;
    }
}

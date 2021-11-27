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
        if (defeatedSpawnerCount >= EnemySpawnPlotManager.SpawnerPlots.Count)
        {
            EnemySpawnPlotManager.NextWave();
        }
    }
    
    private void ResetDefeatedSpawnerCount()
    {
        defeatedSpawnerCount = 0;
    }
}

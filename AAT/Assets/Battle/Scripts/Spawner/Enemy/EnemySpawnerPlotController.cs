using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerPlotController : MonoBehaviour
{
    [SerializeField] private EnemySpawnerController enemySpawnerPrefab;

    public void SetupSpawner(UnitSpawnData spawnData)
    {
        EnemySpawnerController instantiatedSpawner = Instantiate(enemySpawnerPrefab, transform.position, transform.rotation);
        instantiatedSpawner.Setup(spawnData);
    }
}

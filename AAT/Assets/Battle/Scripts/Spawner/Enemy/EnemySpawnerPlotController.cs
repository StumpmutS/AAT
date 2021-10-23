using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerPlotController : MonoBehaviour
{
    [SerializeField] private SectorController sector;
    [SerializeField] private EnemySpawnerController enemySpawnerPrefab;

    public void SetupSpawner(UnitSpawnData spawnData, List<Vector3> patrolPoints)
    {
        EnemySpawnerController instantiatedSpawner = Instantiate(enemySpawnerPrefab, transform.position, transform.rotation);
        instantiatedSpawner.SetPatrol(patrolPoints);
        instantiatedSpawner.Setup(spawnData, sector);
    }
}

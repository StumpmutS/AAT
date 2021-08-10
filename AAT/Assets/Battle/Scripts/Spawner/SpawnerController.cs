using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    private Dictionary<Transform, bool> spawnPointActiveStates = new Dictionary<Transform, bool>();

    private int spawnGroupsAmount;
    private int unitsPerGroupAmount;
    private float spawnTime;
    private float respawnTime;
    private int maxSpawnLocationUse;
    private Vector3 spawnerOffset;
    private GameObject spawnerVisuals;
    private EntityController unitPrefab;

    private int queuedSpawnCount;
    private int currentSpawningCount;
    private List<EntityController> activeEntities = new List<EntityController>();

    private void Awake()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPointActiveStates.Add(spawnPoint, false);
        }
    }

    public void Setup(UnitSpawnData unitSpawnData)
    {
        spawnGroupsAmount = unitSpawnData.SpawnGroupsAmount;
        unitsPerGroupAmount = unitSpawnData.UnitsPerGroupAmount;
        spawnTime = unitSpawnData.SpawnTime;
        respawnTime = unitSpawnData.RespawnTime;
        maxSpawnLocationUse = unitSpawnData.MaxSpawnLocationUse;
        spawnerVisuals = unitSpawnData.SpawnerVisuals;
        spawnerOffset = unitSpawnData.SpawnerOffset;
        unitPrefab = unitSpawnData.UnitPrefab;
        InititializeVisuals();
        InitiliazeSpawning();
    }

    private void InititializeVisuals()
    {
        GameObject instantiatedSpawnerVisuals = Instantiate(spawnerVisuals, gameObject.transform);
        instantiatedSpawnerVisuals.transform.position += spawnerOffset;
    }

    private void InitiliazeSpawning()
    {
        for (int i = 0; i < spawnGroupsAmount; i++)
        {
            CheckSpawnUnitGroup(i % maxSpawnLocationUse);
        }
    }

    private void CheckSpawnUnitGroup(int spawnPointIndex)
    {
        if (currentSpawningCount < maxSpawnLocationUse)
        {
            currentSpawningCount++;
            StartCoroutine(SpawnUnitGroup(spawnPointIndex));
        }
        else
        {
            queuedSpawnCount++;
        }
    }

    private IEnumerator SpawnUnitGroup(int spawnPointIndex)
    {
        spawnPointActiveStates[spawnPoints[spawnPointIndex]] = true;
        yield return new WaitForSeconds(spawnTime);
        SpawnUnits(spawnPointIndex);
        spawnPointActiveStates[spawnPoints[spawnPointIndex]] = false;
        currentSpawningCount--;

        //if there is a unit group in queue use the first inactive spawn point to spawn the queued unit group
        if (queuedSpawnCount > 0)
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                if (!spawnPointActiveStates[spawnPoints[i]])
                {
                    queuedSpawnCount--;
                    currentSpawningCount++;
                    StartCoroutine(SpawnUnitGroup(i));
                    break;
                }
            }
        }
    }

    private void SpawnUnits(int spawnPointIndex)
    {
        for (int i = 0; i < unitsPerGroupAmount; i++)
        {
            EntityController instantiatedUnit = Instantiate(unitPrefab, spawnPointActiveStates.Keys.ToList()[spawnPointIndex].position, Quaternion.identity);
            activeEntities.Add(instantiatedUnit);
        }
    }
}

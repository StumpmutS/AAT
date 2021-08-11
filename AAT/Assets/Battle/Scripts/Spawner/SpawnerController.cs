using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    private Dictionary<Transform, int> spawnPointActiveGroups = new Dictionary<Transform, int>();

    private int _spawnGroupsAmount;
    private int _unitsPerGroupAmount;
    private float _spawnTime;
    private float _respawnTime;
    private int _maxSpawnLocationUse;
    private Vector3 _spawnerOffset;
    private GameObject _spawnerVisuals;
    private UnitController _unitPrefab;

    private int queuedSpawnCount;
    private int currentSpawningCount;
    private List<List<UnitController>> activeUnitGroups = new List<List<UnitController>>();
    private int takenUnitGroupCount;

    private void Awake()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPointActiveGroups.Add(spawnPoint, -1);
        }
    }

    public void Setup(UnitSpawnData unitSpawnData)
    {
        _spawnGroupsAmount = unitSpawnData.SpawnGroupsAmount;
        _unitsPerGroupAmount = unitSpawnData.UnitsPerGroupAmount;
        _spawnTime = unitSpawnData.SpawnTime;
        _respawnTime = unitSpawnData.RespawnTime;
        _maxSpawnLocationUse = unitSpawnData.MaxSpawnLocationUse;
        _spawnerVisuals = unitSpawnData.SpawnerVisuals;
        _spawnerOffset = unitSpawnData.SpawnerOffset;
        _unitPrefab = unitSpawnData.UnitPrefab;

        for (int i = 0; i < _spawnGroupsAmount; i++)
        {
            AddEmptyUnitGroup();
        }
        InititializeVisuals();
        InitiliazeSpawning();
    }

    private void InititializeVisuals()
    {
        GameObject instantiatedSpawnerVisuals = Instantiate(_spawnerVisuals, gameObject.transform);
        instantiatedSpawnerVisuals.transform.position += _spawnerOffset;
    }

    private void InitiliazeSpawning()
    {
        for (int i = 0; i < _spawnGroupsAmount; i++)
        {
            QueueUnitGroup(_spawnTime);
        }
    }

    private void QueueUnitGroup(float spawnTime)
    {
        print("queue being called");
        if (currentSpawningCount < _maxSpawnLocationUse)
        {
            currentSpawningCount++;
            SpawnUnitGroup(spawnTime);
            return;
        }
        queuedSpawnCount++;
    }

    private void SpawnUnitGroup(float spawnTime)
    {
        print("group being called");
        int spawnPointIndex = GetFirstInactiveSpawnerIndex();
        int groupIndex = GetEmptyGroup();
        spawnPointActiveGroups[spawnPoints[spawnPointIndex]] = groupIndex;
        for (int i = 0; i < _unitsPerGroupAmount; i++) 
        {
            StartCoroutine(SpawnUnit(spawnPointIndex, groupIndex, i, spawnTime));
        }
    }

    private IEnumerator SpawnUnit(int spawnPointIndex, int groupIndex, int unitIndex, float spawnTime)
    {
        print("unit being called");
        yield return new WaitForSeconds(spawnTime);

        spawnPointActiveGroups[spawnPoints[spawnPointIndex]] = -1;//////////////////////////
        currentSpawningCount--;

        //if there is a unit group in queue use the first inactive spawn point to spawn the queued unit group
        if (queuedSpawnCount > 0)
        {
            int inactiveSpawnerIndex = GetFirstInactiveSpawnerIndex();
            if (inactiveSpawnerIndex > -1)
            {
                queuedSpawnCount--;
                currentSpawningCount++;
                SpawnUnitGroup(spawnTime);
            }
        }
        UnitController instantiatedUnit = Instantiate(_unitPrefab, spawnPoints[spawnPointIndex].position, Quaternion.identity);
        activeUnitGroups[groupIndex].Add(instantiatedUnit);
        instantiatedUnit.SetupDeath(ActiveUnitDeathHandler, groupIndex, unitIndex);
    }

    private void ActiveUnitDeathHandler(int groupIndex, int unitIndex)
    {
        print("respawn " + groupIndex + ", " + unitIndex);
        //check for respawn unit or group
    }

    #region Helper Methods
    private int GetFirstInactiveSpawnerIndex()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnPointActiveGroups[spawnPoints[i]] < 0)
            {
                return i;
            }
        }
        return -1;
    }

    public void AddEmptyUnitGroup()
    {
        activeUnitGroups.Add(new List<UnitController>());
    }

    private int GetEmptyGroup()
    {
        for (int i = 0; i < activeUnitGroups.Count; i++)
        {
            if (activeUnitGroups[i].Count == 0 && i >= takenUnitGroupCount)
            {
                takenUnitGroupCount++;
                return i;
            }
        }
        return -1;
    }
    #endregion
}

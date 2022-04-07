using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : BaseSpawnerController
{
    private List<Vector3> _patrolPoints;
    private int deadUnitGroupsCount;

    public void SetPatrol(List<Vector3> patrolPoints)
    {
        _patrolPoints = patrolPoints;
    }

    protected override void ActiveUnitDeathHandler(int groupIndex)
    {
        if (_activeUnitGroups[groupIndex].Units.Count <= 0)
        {
            deadUnitGroupsCount++;
            if (deadUnitGroupsCount >= _spawnGroupsAmount)
            {
                EnemySpawnerManager.SpawnerDefeatHandler();
            }
        }
    }

    protected override IEnumerator SpawnUnitsCoroutine(float spawnTime, int groupIndex)
    {
        yield return new WaitForSeconds(spawnTime);
        for (int i = 0; i < _unitGroupNumbers[groupIndex]; i++)
        {
            UnitController instantiatedUnit = Instantiate(_unitPrefab, _activeUnitGroups[groupIndex].transform.position, Quaternion.identity);
            _activeUnitGroups[groupIndex].AddUnit(instantiatedUnit);
            if (_patrolPoints != null) instantiatedUnit.SetPatrolPoints(_patrolPoints);
        }
    }
}

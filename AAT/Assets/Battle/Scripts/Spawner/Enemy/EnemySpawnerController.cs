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
        if (activeUnitGroups[groupIndex].Units.Count <= 0)
        {
            deadUnitGroupsCount++;
            print(deadUnitGroupsCount);
            if (deadUnitGroupsCount >= _spawnGroupsAmount)
            {
                print("all dead");
                EnemySpawnerManager.SpawnerDefeatHandler();
            }
        }
    }

    protected override IEnumerator SpawnUnitsCoroutine(float spawnTime, int groupIndex)
    {
        yield return new WaitForSeconds(spawnTime);
        for (int i = 0; i < unitGroupNumbers[groupIndex]; i++)
        {
            UnitController instantiatedUnit = Instantiate(_unitPrefab, activeUnitGroups[groupIndex].transform.position, Quaternion.identity);
            activeUnitGroups[groupIndex].AddUnit(instantiatedUnit);
            if (_patrolPoints != null) instantiatedUnit.SetPatrolPoints(_patrolPoints);
        }
    }
}

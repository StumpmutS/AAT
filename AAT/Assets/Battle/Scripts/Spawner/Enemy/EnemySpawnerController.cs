using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : BaseSpawnerController
{
    private int deadUnitGroupsCount;

    protected override void ActiveUnitDeathHandler(int groupIndex)
    {
        if (activeUnitGroups[groupIndex].Units.Count <= 0)
        {
            deadUnitGroupsCount++;
            if (deadUnitGroupsCount >= _spawnGroupsAmount)
            {
                EnemySpawnerManager.SpawnerDefeatHandler();
            }
        }
    }
}

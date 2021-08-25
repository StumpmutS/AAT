using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : BaseSpawnerController
{
    protected override void ActiveUnitDeathHandler(int groupIndex)
    {
        if (activeUnitGroups[groupIndex].Units.Count <= 0)
        {
            RespawnUnitGroup(groupIndex);
        }
        else
        {
            RespawnUnit(groupIndex);
        }
    }

    protected override void Select()
    {
        base.Select();
        foreach (var unitGroup in activeUnitGroups)
        {
            unitGroup.SelectGroup();
        }
        _upgradesUIContainer.gameObject.SetActive(true);
    }

    protected override void Deselect()
    {
        foreach (var unitGroup in activeUnitGroups)
        {
            unitGroup.DeselectGroup();
        }
        _upgradesUIContainer.gameObject.SetActive(false);
    }
}

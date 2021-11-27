using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountableUnitController : BaseMountableController
{
    [SerializeField] private UnitController unit;
    [SerializeField] private MountableUnitData mountableUnitData;

    public override BaseMountableDataInfo ReturnData()
    {
        return mountableUnitData.MountableInfo;
    }

    public override void ActivateMounted(BaseUnitStatsData stats)
    {
        unit.ModifyStats(mountableUnitData.MountableInfo.SelfModifier);
        unit.ModifyStats(stats);
    }

    public override void DeactivateMounted(BaseUnitStatsData stats)
    {
        unit.ModifyStats(mountableUnitData.MountableInfo.SelfModifier, false);
        unit.ModifyStats(stats, false);
    }
}

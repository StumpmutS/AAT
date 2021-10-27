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

    public override void ActivateMounted()
    {
        unit.ModifyStats(mountableUnitData.MountableInfo.TransportingModifier);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountableUnitController : BaseMountableController
{
    [SerializeField] private MountableUnitData mountableUnitData;

    public override BaseMountableDataInfo ReturnData()
    {
        return mountableUnitData.MountableInfo;
    }
}

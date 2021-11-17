using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountableData<T> : ScriptableObject where T : BaseMountableDataInfo
{
    public T MountableInfo;
}

[Serializable]
public class BaseMountableDataInfo
{
    public float MountRange;
    public UnitStatsDataInfo MountedUnitModifier;
}
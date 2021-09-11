using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Base/Mountable Unit Data")]
public class MountableUnitData : ScriptableObject
{
    public float MountRange;
    public UnitStatsDataInfo MountedUnitModifier;
    public UnitStatsDataInfo TransportingModifier;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Base/Transportable Unit Data")]
public class TransportableData : ScriptableObject
{
    public BaseUnitStatsData SelfStatsAlter;
    public BaseUnitStatsData MountStatsAlter;
}

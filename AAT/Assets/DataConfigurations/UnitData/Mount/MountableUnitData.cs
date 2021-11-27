using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Base/Mountable Unit Data")]
public class MountableUnitData : MountableData<MountableUnitDataInfo>
{
}

[Serializable]
public class MountableUnitDataInfo : BaseMountableDataInfo
{
    public BaseUnitStatsData SelfModifier;
}

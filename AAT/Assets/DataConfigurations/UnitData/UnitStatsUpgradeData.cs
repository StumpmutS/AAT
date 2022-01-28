using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Base/Unit Upgrade Data")]
public class UnitStatsUpgradeData : ScriptableObject //TODO:
{
    public BaseUnitStatsData baseUnitStatsDataInfo;
    
    public void Upgrade(UnitStatsModifierManager unitStatsModifier)
    {
        unitStatsModifier.ModifyStats(baseUnitStatsDataInfo);
    }
}

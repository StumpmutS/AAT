using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Base/Unit Upgrade Data")]
public class UnitStatsUpgradeData : ScriptableObject
{
    private UnitStatsModifierManager statsModifier;

    public UnitStatsDataInfo UnitStatsDataInfo;
    
    public void Upgrade(UnitStatsModifierManager unitStatsModifier)
    {
        statsModifier = unitStatsModifier;
        statsModifier.ModifyStats(UnitStatsDataInfo);
    }
}

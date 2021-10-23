using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Passives/Sector Power Stats Passive Component")]
public class SectorPowerStatsBuffPassiveComponent : SectorPowerDeterminedPassiveComponent
{
    [SerializeField] private List<UnitStatsData> unitStatsUpgradeData;

    protected override void ActivateThresholdIndex(int index)
    {
        _unit.ModifyStats(unitStatsUpgradeData[index].UnitStatsDataInfo);
    }

    protected override void DeactivateThresholdIndex(int index)
    {
        _unit.ModifyStats(unitStatsUpgradeData[index].UnitStatsDataInfo, false);
    }
}

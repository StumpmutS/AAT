using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Passives/Sector Power Stats Passive Component")]
public class SectorPowerStatsBuffPassiveComponent : SectorPowerDeterminedPassiveComponent
{
    [SerializeField] private List<BaseUnitStatsData> unitStatsUpgradeData;

    protected override void ActivateThresholdIndex(SectorController sector, int index)
    {
        foreach (var unit in _unitsBySector[sector])
        {
            unit.ModifyStats(unitStatsUpgradeData[index]);
        }
    }

    protected override void DeactivateThresholdIndex(SectorController sector, int index)
    {
        foreach (var unit in _unitsBySector[sector])
        {
            unit.ModifyStats(unitStatsUpgradeData[index], false);
        }
    }
}

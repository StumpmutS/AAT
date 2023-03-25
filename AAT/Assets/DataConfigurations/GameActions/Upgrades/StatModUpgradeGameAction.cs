using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Actions/Upgrades/StatMod")]
public class StatModUpgradeGameAction : UpgradeGameAction
{
    [SerializeField] private BaseUnitStatsData unitStatsData;
    
    public override void PerformAction(GameActionInfo info)
    {
        if (info.Target.Hit.TryGetComponent<StatsManager>(out var stats))
        {
            stats.ModifyStats(unitStatsData);
        }
    }

    public override void StopAction(GameActionInfo info)
    {
        if (info.Target.Hit.TryGetComponent<StatsManager>(out var stats))
        {
            stats.ModifyStats(unitStatsData, false);
        }
    }
}

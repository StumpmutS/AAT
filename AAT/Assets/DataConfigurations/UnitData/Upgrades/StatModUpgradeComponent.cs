using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Upgrades/StatMod")]
public class StatModUpgradeComponent : UnitUpgradeComponent
{
    [SerializeField] private BaseUnitStatsData unitStatsData;
    
    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        unit.Stats.ModifyStats(unitStatsData);
    }
}

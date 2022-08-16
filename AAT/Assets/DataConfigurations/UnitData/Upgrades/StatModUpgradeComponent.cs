using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModUpgradeComponent : UnitUpgradeComponent
{
    [SerializeField] private BaseUnitStatsData unitStatsData;
    
    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        unit.Stats.ModifyStats(unitStatsData);
    }
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Base/Unit Upgrade Data")]
public class UnitStatsUpgradeData : ScriptableObject
{
    [SerializeField] private List<UnitUpgradeComponent> upgradeComponents;
    [SerializeField] private List<VisualComponent> visuals;

    public void LogicallyUpgradeUnit(UnitController unit)
    {
        foreach (var component in upgradeComponents)
        {
            component.ActivateComponent(unit);
        }
    }

    public void VisuallyUpgradeUnit(UnitController unit)
    {
        foreach (var component in visuals)
        {
            component.ActivateComponent(unit);
        }
    }
}

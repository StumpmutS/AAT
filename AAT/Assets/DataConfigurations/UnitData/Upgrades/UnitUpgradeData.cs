using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Upgrades/Unit Upgrade Data")]
public class UnitUpgradeData : ScriptableObject
{
    [SerializeField] private List<UnitUpgradeComponent> upgradeComponents;
    [SerializeField] private List<UnitVisualComponent> visuals;

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

    public void LogicallyDowngradeUnit(UnitController unit)
    {
        foreach (var component in upgradeComponents)
        {
            component.DeactivateComponent(unit);
        }
    }

    public void VisuallyDowngradeUnit(UnitController unit)
    {
        foreach (var component in visuals)
        {
            component.DeactivateComponent(unit);
        }
    }
}

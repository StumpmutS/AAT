using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Upgrades/Unit Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [SerializeField] private UserActionInfo userActionInfo;
    public UserActionInfo UserActionInfo => userActionInfo;
    [SerializeField] private List<UpgradeGameAction> upgradeGameActions;
    [SerializeField] private List<VisualGameAction> visuals;

    public void LogicallyUpgrade(IGameActionInfoGetter getter)
    {
        GameActionRunner.Instance.PerformActions(upgradeGameActions, getter);
    }

    public void VisuallyUpgrade(IGameActionInfoGetter getter)
    {
        GameActionRunner.Instance.PerformActions(visuals, getter);
    }
}
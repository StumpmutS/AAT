using System.Collections.Generic;
using UnityEngine;

public class SpawnerUpgradeManager : GroupUpgradeManager, IActionCreator
{
    [SerializeField] private string userActionCategory;

    public List<UserAction> GetActions()
    {
        if (_upgradeIndexMap.Length >= _upgrades.Count) return new List<UserAction>();
        
        List<UserAction> actions = new();
        var nextUpgrades = _upgrades[_upgradeIndexMap.Length];

        for (int i = 0; i < nextUpgrades.Count; i++)
        {
            actions.Add(new UserAction(this, userActionCategory, ESubCategory.Upgrades, nextUpgrades[i].UserActionInfo.Label,
                nextUpgrades[i].UserActionInfo.Icon, TryUpgrade, HandleActionDeselected, i, nextUpgrades[i].UserActionInfo.KeyCode));
        }

        return actions;
    }

    private void TryUpgrade(object obj)
    {
        if (!Object.HasInputAuthority || obj is not int i) return;
        RpcUpgrade(i);
    }
    
    private void HandleActionDeselected(object _) { }
}
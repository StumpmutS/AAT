using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using Utility.Scripts;

public abstract class UpgradeManager : NetworkBehaviour, IGameActionInfoGetter
{
    [SerializeField] private List<UpgradeDataListWrapper> upgrades;

    //Each int added is a record of an upgrade, where the added int is the index of the selected upgrade
    [Networked(OnChanged = nameof(OnUpgradeIndexChanged)), Capacity(8)]
    protected NetworkArray<int> _upgradeIndexMap { get; } = MakeInitializer(new int[]
    { -1, -1, -1, -1, -1, -1, -1, -1 });

    public static void OnUpgradeIndexChanged(Changed<UpgradeManager> changed)
    {
        changed.LoadOld();
        var previousIndex = changed.Behaviour._upgradeIndexMap.Length - 1;
        changed.LoadNew();
        var behaviour = changed.Behaviour;
        var indexMap = behaviour._upgradeIndexMap;
        
        for (int i = previousIndex + 1; i <= indexMap.Length - 1; i++)
        {
            changed.Behaviour._upgrades[i][indexMap.Get(i)].VisuallyUpgrade(behaviour);
        }
    }

    protected List<List<UpgradeData>> _upgrades { get; private set; }

    protected virtual void Awake()
    {
        if (upgrades == null) return;
        Init(upgrades.Select(list => list.UpgradeData).ToList(), new List<int>());
    }

    private void Start()
    {
        for (int i = 0; i < _upgradeIndexMap.Length; i++)
        {
            VisuallyUpgrade(i, _upgradeIndexMap[i]);
        }
    }

    public void Init(List<List<UpgradeData>> upgradeData, List<int> currentIndexMap)
    {
        _upgrades = upgradeData;
        for (int i = 0; i < currentIndexMap.Count; i++)
        {
            _upgradeIndexMap.Set(i, currentIndexMap[i]);
            LogicallyUpgrade(i, currentIndexMap[i]);
        }
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RpcUpgrade(int selectionIndex)
    {
        var nextUpgradeIndex = _upgradeIndexMap.Length - 1;
        if (nextUpgradeIndex >= _upgrades.Count) return;
        if (Runner.IsServer)
        {
            LogicallyUpgrade(nextUpgradeIndex, selectionIndex);
            _upgradeIndexMap.Set(nextUpgradeIndex, selectionIndex);
        }
        
        VisuallyUpgrade(nextUpgradeIndex, selectionIndex);
    }

    private void LogicallyUpgrade(int upgradeIndex, int selectionIndex)
    {
        if (!Runner.IsServer || upgradeIndex >= _upgrades.Count) return;

        _upgrades[upgradeIndex][selectionIndex].LogicallyUpgrade(this);
    }

    private void VisuallyUpgrade(int upgradeIndex, int selectionIndex)
    {
        if (upgradeIndex >= _upgrades.Count) return;
        
        _upgrades[upgradeIndex][selectionIndex].VisuallyUpgrade(this);
    }

    public abstract IEnumerable<GameActionInfo> GetInfo();
}
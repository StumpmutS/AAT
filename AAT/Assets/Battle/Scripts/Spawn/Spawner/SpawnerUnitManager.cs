using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerUnitManager : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnUpgradeIndexChanged))] private int _currentUpgradeIndex { get; set; }
    public static void OnUpgradeIndexChanged(Changed<SpawnerUnitManager> changed)
    {
        for (int i = 0; i < changed.Behaviour._currentUpgradeIndex; i++)
        {
            foreach (var unit in changed.Behaviour._spawner.Groups.SelectMany(g => g.Units))
            {
                changed.Behaviour._upgradeData[i].VisuallyUpgradeUnit(unit);
            }
        }
    }
    
    private SpawnerController _spawner;
    private List<UnitGroupController> _groups => _spawner.Groups;
    private List<UnitStatsUpgradeData> _upgradeData => _spawner.SpawnData.UnitStatsUpgradeData;
    
    private void Awake()
    {
        _spawner = GetComponent<SpawnerController>();
        _spawner.OnUnitsSpawned += HandleUnitsSpawned;
    }

    public void UpgradeUnits()
    {
        if (!Runner.IsServer || _currentUpgradeIndex >= _upgradeData.Count) return;
        
        foreach (var group in _groups)
        {
            foreach (var unit in group.Units)
            {
                _upgradeData[_currentUpgradeIndex].LogicallyUpgradeUnit(unit);
            }
        }

        _currentUpgradeIndex++;
    }

    private void HandleUnitsSpawned(IEnumerable<UnitController> units)
    {
        for (int i = 0; i < _currentUpgradeIndex; i++)
        {
            foreach (var unit in units)
            {
                _upgradeData[i].LogicallyUpgradeUnit(unit);
            }
        }
    }

    private void OnDestroy()
    {
        if (_spawner != null) _spawner.OnUnitsSpawned -= HandleUnitsSpawned;
    }
}

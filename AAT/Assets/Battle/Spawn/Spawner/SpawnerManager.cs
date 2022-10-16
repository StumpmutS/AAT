using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class SpawnerManager : NetworkBehaviour
{
    public static SpawnerManager Instance { get; private set; }
    public Dictionary<GroupSpawnerController, Tuple<SpawnerUnitManager, SpawnerVisualsController>> Spawners { get; private set; } = new();
    
    private HashSet<SpawnerUnitManager> _selectedSpawnerUnitManagers = new();

    private void Awake()
    {
        Instance = this;
    }

    public override void Spawned()
    {
        Object.AssignInputAuthority(Runner.LocalPlayer);
    }

    public void Upgrade()
    {
        if (_selectedSpawnerUnitManagers.Count < 1) return;
        
        var selected = _selectedSpawnerUnitManagers.Where(m => m.Object.HasInputAuthority).Select(m => m.Id).ToArray();
        RpcTryUpgrade(selected);
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RpcTryUpgrade(NetworkBehaviourId[] ids, RpcInfo info = default)
    {
        HashSet<SpawnerUnitManager> managers = new();
        foreach (var id in ids)
        {
            if (Runner.TryFindBehaviour(id, out var behaviour))
            {
                managers.Add(behaviour.GetComponent<SpawnerUnitManager>());
            }
        }

        foreach (var spawnerUnitManager in managers.Where(m => m.Object.InputAuthority == info.Source))
        {
            spawnerUnitManager.UpgradeUnits();
        }
    }

    public void AddSpawner(GroupSpawnerController spawner)
    {
        var visuals = spawner.GetComponentInChildren<SpawnerVisualsController>();
        Spawners[spawner] = new Tuple<SpawnerUnitManager, SpawnerVisualsController>(spawner.GetComponent<SpawnerUnitManager>(), visuals);
        
        if (visuals != null)
        {
            visuals.OnSelect += HandleSelected;
            visuals.OnDeselect += HandleDeselected;
        }
    }

    public void RemoveSpawner(GroupSpawnerController spawner)
    {
        var visuals = Spawners[spawner].Item2;
        if (visuals != null)
        {
            visuals.OnSelect -= HandleSelected;
            visuals.OnDeselect -= HandleDeselected;
        }
    }

    private void HandleSelected(GroupSpawnerController spawner)
    {
        _selectedSpawnerUnitManagers.Add(Spawners[spawner].Item1);
    }

    private void HandleDeselected(GroupSpawnerController spawner)
    {
        _selectedSpawnerUnitManagers.Remove(Spawners[spawner].Item1);
    }
}

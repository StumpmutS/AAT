using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class UnitGroupController : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnUnitsChanged)), Capacity(16)] public NetworkLinkedList<UnitController> Units => default;
    public static void OnUnitsChanged(Changed<UnitGroupController> changed)
    {
        var group = changed.Behaviour;
        foreach (var unit in group.Units)
        {
            if (!group._localSetup.Contains(unit))
            {
                group.SetupUnit(unit);
            }
        }

        var unitsToRemove = group._localSetup.Where(u => !group.Units.Contains(u)).ToHashSet();
        foreach (var unit in unitsToRemove)
        {
            group.UnSetupUnit(unit);
        }
    }

    private HashSet<UnitController> _localSetup = new();

    private SpawnerController _spawner;
    private bool _outlined;

    public void Setup(SpawnerController spawner, int groupIndex)
    {
        //spawner.OnModifyStats += ModifyUnitStats;
    }

    public void Die()
    {
        if (!Runner.IsServer) return;

        foreach (var unit in Units)
        {
            unit.GetComponent<UnitDeathController>().Die();
        }
        
        Runner.Despawn(Object);
    }

    private void UnitDeathHandler(UnitController unit)
    {
        RemoveUnit(unit);
        if (_spawner != null) _spawner.QueueUnitGroup(this);
    }

    public void AddUnit(UnitController unit)
    {
        Units.Add(unit);
        if (Units.Any(u => u.NetworkSelected))
        {
            unit.OutlineSelectable.CallSelectOverrideUICheck();
        } 
        else if (_outlined)
        {
            unit.OutlineSelectable.Outline();
        }
    }
    
    public void RemoveUnit(UnitController unit)
    {
        Units.Remove(unit);
        
        if (Units.Count <= 0)
        {
            _outlined = false;
        }
    }

    public void SelectGroup()
    {
        foreach (var unit in Units)
        {
            unit.OutlineSelectable.CallSelect();
        }
    }

    public void DeselectGroup()
    {
        foreach (var unit in Units)
        {
            unit.OutlineSelectable.CallDeselect();
        }
    }

    private void OutlineGroup()
    {
        if (_outlined) return;
        _outlined = true;
        foreach (var unit in Units)
        {
            unit.OutlineSelectable.Outline();
        }
    }

    private void RemoveGroupOutline()
    {
        if (!_outlined) return;
        _outlined = false;
        foreach (var unit in Units)
        {
            unit.OutlineSelectable.RemoveOutline();
        }
    }

    private void SetupUnit(UnitController unit)
    {
        if (!_localSetup.Add(unit)) return;
        
        unit.OutlineSelectable.OnSelect += SelectGroup;
        unit.OutlineSelectable.OnOutline += OutlineGroup;
        unit.OutlineSelectable.OnRemoveOutline += RemoveGroupOutline;
        unit.OnDeath += UnitDeathHandler;
    }

    private void UnSetupUnit(UnitController unit)
    {
        if (!_localSetup.Remove(unit)) return;
        
        unit.OutlineSelectable.OnSelect -= SelectGroup;
        unit.OutlineSelectable.OnOutline -= OutlineGroup;
        unit.OutlineSelectable.OnRemoveOutline -= RemoveGroupOutline;
        unit.OnDeath -= UnitDeathHandler;
    }

    private void ModifyUnitStats(BaseUnitStatsData baseUnitStatsDataInfo)
    {
        foreach (var unit in Units)
        {
            unit.ModifyStats(baseUnitStatsDataInfo);
        }
    }

    public void SetChaseStates(bool value)
    {
        foreach (var unit in Units)
        {
            unit.SetChaseState(value);
        }
    }

    public bool GetChaseStates()
    {
        int chaseStateCompare = 0;
        foreach (var unit in Units)
        {
            if (unit.ChaseState) chaseStateCompare++;
            else chaseStateCompare--;
        }
        if (chaseStateCompare == Units.Count) return chaseStateCompare >= 0;
        return false;
    }
}

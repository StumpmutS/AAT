using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class UnitGroup : Group
{
    [Networked(OnChanged = nameof(OnUnitsChanged)), Capacity(16)] public NetworkLinkedList<UnitController> Units => default;
    public static void OnUnitsChanged(Changed<UnitGroup> changed)
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

    private GroupSpawnerController _spawner;
    private bool _outlined;

    public void Setup(GroupSpawnerController spawner, int groupIndex)
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
        throw new NotImplementedException();
        //if (_spawner != null) _spawner.QueueUnitGroup(this);
    }

    public void AddUnit(UnitController unit)
    {
        throw new NotImplementedException();
        Units.Add(unit);
        if (Units.Any(u => u.NetworkSelected))
        {
            unit.Selectable.CallSelectOverrideUICheck();
        } 
        /*else if (_outlined)
        {
            unit.Selectable.Outline();
        }*/
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
            unit.Selectable.CallSelect();
        }
    }

    public void DeselectGroup()
    {
        foreach (var unit in Units)
        {
            unit.Selectable.CallDeselect();
        }
    }

    private void OutlineGroup()
    {
        throw new NotImplementedException();
        if (_outlined) return;
        _outlined = true;/*
        foreach (var unit in Units)
        {
            unit.Selectable.Outline();
        }*/
    }

    private void RemoveGroupOutline()
    {
        throw new NotImplementedException();
        if (!_outlined) return;
        _outlined = false;/*
        foreach (var unit in Units)
        {
            unit.Selectable.RemoveOutline();
        }*/
    }

    private void SetupUnit(UnitController unit)
    {
        throw new NotImplementedException();
        if (!_localSetup.Add(unit)) return;
        
        unit.Selectable.OnSelect.AddListener(SelectGroup);/*
        unit.Selectable.OnOutline += OutlineGroup;
        unit.Selectable.OnRemoveOutline += RemoveGroupOutline;*/
        unit.OnDeath += UnitDeathHandler;
    }

    private void UnSetupUnit(UnitController unit)
    {
        throw new NotImplementedException();
        if (!_localSetup.Remove(unit)) return;
        
        unit.Selectable.OnSelect.RemoveListener(SelectGroup);/*
        unit.Selectable.OnOutline -= OutlineGroup;
        unit.Selectable.OnRemoveOutline -= RemoveGroupOutline;*/
        unit.OnDeath -= UnitDeathHandler;
    }

    private void ModifyUnitStats(BaseUnitStatsData baseUnitStatsDataInfo)
    {
        foreach (var unit in Units)
        {
            unit.ModifyStats(baseUnitStatsDataInfo);
        }
    }
}

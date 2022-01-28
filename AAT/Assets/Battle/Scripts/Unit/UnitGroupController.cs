using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroupController : MonoBehaviour
{
    private List<UnitController> units = new List<UnitController>();
    public List<UnitController> Units => units;

    private Action<int> _unitDeathCallback;
    private int _groupIndex;

    private bool selected = false; 
    private bool outlined = false;

    public void Setup(Action<int> deathCallback, BaseSpawnerController spawner, int groupIndex)
    {
        spawner.OnModifyStats += ModifyUnitStats;
        _unitDeathCallback = deathCallback;
        _groupIndex = groupIndex;
    }

    private void UnitDeathHandler(UnitController unit)
    {
        RemoveUnit(unit);
        if (units.Count <= 0)
        {
            selected = false;
            outlined = false;
        }
        _unitDeathCallback?.Invoke(_groupIndex);
    }

    public void AddUnit(UnitController unit)
    {
        units.Add(unit);
        SetupUnit(unit);
        if (selected)
        {
            unit.CallSelect();
        } 
        else if (outlined)
        {
            unit.Outline();
        }
    }
    
    private void RemoveUnit(UnitController unit)
    {
        units.Remove(unit);
        UnSetupUnit(unit);
    }

    public void SelectGroup()
    {
        if (selected) return;
        selected = true;
        foreach (var unit in units)
        {
            unit.CallSelect();
        }
        UnitGroupSelectionManager.Instance.AddUnitGroup(this);
    }

    public void DeselectGroup()
    {
        if (!selected) return;
        selected = false;
        foreach (var unit in units)
        {
            unit.CallDeselect();
        }
        UnitGroupSelectionManager.Instance.RemoveUnitGroup(this);
    }

    public void OutlineGroup()
    {
        if (outlined) return;
        outlined = true;
        foreach (var unit in units)
        {
            unit.Outline();
        }
    }

    public void RemoveGroupOutline()
    {
        if (!outlined) return;
        outlined = false;
        foreach (var unit in units)
        {
            unit.RemoveOutline();
        }
    }

    private void SetupUnit(UnitController unit)
    {
        unit.SetGroup(this);
        unit.OnSelect += SelectGroup;
        unit.OnDeselect += DeselectGroup;
        unit.OnOutline += OutlineGroup;
        unit.OnRemoveOutline += RemoveGroupOutline;
        unit.OnDeath += UnitDeathHandler;
    }

    private void UnSetupUnit(UnitController unit)
    {
        unit.OnSelect -= SelectGroup;
        unit.OnDeselect -= DeselectGroup;
        unit.OnOutline -= OutlineGroup;
        unit.OnRemoveOutline -= RemoveGroupOutline;
        unit.OnDeath -= UnitDeathHandler;
    }

    private void ModifyUnitStats(BaseUnitStatsData baseUnitStatsDataInfo)
    {
        foreach (var unit in units)
        {
            unit.ModifyStats(baseUnitStatsDataInfo);
        }
    }

    public void SetChaseStates(bool value)
    {
        foreach (var unit in units)
        {
            unit.SetChaseState(value);
        }
    }

    public bool GetChaseStates()
    {
        int chaseStateCompare = 0;
        foreach (var unit in units)
        {
            if (unit.ChaseState) chaseStateCompare++;
            else chaseStateCompare--;
        }
        if (chaseStateCompare == units.Count) return chaseStateCompare >= 0 ? true : false;
        else return false;
    }
}

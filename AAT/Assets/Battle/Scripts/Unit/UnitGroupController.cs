using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroupController : MonoBehaviour //TODO
{
    public List<UnitController> Units { get; } = new();

    private Action<int> _unitDeathCallback;
    private int _groupIndex;

    private bool _selected; 
    private bool _outlined;

    public void Setup(Action<int> deathCallback, BaseSpawnerController spawner, int groupIndex)
    {
        spawner.OnModifyStats += ModifyUnitStats;
        _unitDeathCallback = deathCallback;
        _groupIndex = groupIndex;
    }

    private void UnitDeathHandler(UnitController unit)
    {
        RemoveUnit(unit);
        if (Units.Count <= 0)
        {
            _selected = false;
            _outlined = false;
        }
        _unitDeathCallback?.Invoke(_groupIndex);
    }

    public void AddUnit(UnitController unit)
    {
        Units.Add(unit);
        SetupUnit(unit);
        if (_selected)
        {
            unit.OutlineSelectable.CallSelect();
        } 
        else if (_outlined)
        {
            unit.OutlineSelectable.Outline();
        }
    }
    
    private void RemoveUnit(UnitController unit)
    {
        Units.Remove(unit);
        UnSetupUnit(unit);
    }

    public void SelectGroup()
    {
        if (_selected) return;
        _selected = true;
        foreach (var unit in Units)
        {
            unit.OutlineSelectable.CallSelect();
        }
        UnitGroupSelectionManager.Instance.AddUnitGroup(this);
    }

    public void DeselectGroup()
    {
        if (!_selected) return;
        _selected = false;
        foreach (var unit in Units)
        {
            unit.OutlineSelectable.CallDeselect();
        }
        UnitGroupSelectionManager.Instance.RemoveUnitGroup(this);
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
        unit.SetGroup(this, Units.IndexOf(unit));
        unit.OutlineSelectable.OnSelect += SelectGroup;
        unit.OutlineSelectable.OnDeselect += DeselectGroup;
        unit.OutlineSelectable.OnOutline += OutlineGroup;
        unit.OutlineSelectable.OnRemoveOutline += RemoveGroupOutline;
        unit.OnDeath += UnitDeathHandler;
    }

    private void UnSetupUnit(UnitController unit)
    {
        unit.OutlineSelectable.OnSelect -= SelectGroup;
        unit.OutlineSelectable.OnDeselect -= DeselectGroup;
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

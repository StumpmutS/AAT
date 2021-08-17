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

    public void Setup(Action<int> deathCallback, Action<List<Stat>, List<float>, ETransportationType, EAttackType, EMovementType> modifyStatEvent, int groupIndex)
    {
        modifyStatEvent += ModifyUnitStats;
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
            unit.Select();
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
            unit.Select();
        }
    }

    public void DeselectGroup()
    {
        if (!selected) return;
        selected = false;
        foreach (var unit in units)
        {
            unit.Deselect();
        }
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

    #region StatModifierMethods
    private void ModifyUnitStats(List<Stat> stats, List<float> amounts, ETransportationType transportationType = ETransportationType.None, EAttackType attackType = EAttackType.None, EMovementType movementType = EMovementType.None)
    {
        foreach (var unit in units)
        {
            unit.ModifyStats(stats, amounts, transportationType, attackType, movementType);
        }
    }
    #endregion
}

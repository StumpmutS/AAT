using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    public HashSet<UnitController> Units { get; private set; } = new();
    public HashSet<UnitController> SelectedUnits { get; private set; } = new();

    public event Action<UnitController> OnUnitSelected = delegate { };
    public event Action<UnitController> OnUnitDeselected = delegate { };

    private void Awake()
    {
        Instance = this;
    }

    public void AddUnit(UnitController unit)
    {
        if (!unit.Runner.IsServer) return;
        
        unit.OnDeath += RemoveUnit;
        Units.Add(unit);
    }

    private void RemoveUnit(UnitController unit)
    {
        if (!unit.Runner.IsServer) return;
        
        unit.OnDeath -= RemoveUnit;
        Units.Remove(unit);
    }

    public void AddSelectedUnit(UnitController unit)
    {
        if (!unit.Runner.IsServer) return;
        
        SelectedUnits.Add(unit);
        OnUnitSelected.Invoke(unit);
    }

    public void RemoveSelectedUnit(UnitController unit)
    {
        if (!unit.Runner.IsServer) return;
        
        SelectedUnits.Remove(unit);
        OnUnitDeselected.Invoke(unit);
    }
}
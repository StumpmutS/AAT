using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    public HashSet<UnitController> Units { get; private set; } = new HashSet<UnitController>();
    public HashSet<UnitController> SelectedUnits { get; private set; } = new HashSet<UnitController>();

    public event Action<UnitController> OnUnitAdded = delegate { };
    public event Action<UnitController> OnUnitRemoved = delegate { };

    private void Awake()
    {
        Instance = this;
    }

    public void AddUnit(UnitController unit)
    {
        unit.OnDeath += RemoveUnit;
        Units.Add(unit);
        OnUnitAdded.Invoke(unit);
    }

    private void RemoveUnit(UnitController unit)
    {
        unit.OnDeath -= RemoveUnit;
        Units.Remove(unit);
        OnUnitRemoved.Invoke(unit);
    }

    public void AddSelectedUnit(UnitController unit)
    {
        SelectedUnits.Add(unit);
    }

    public void RemoveSelectedUnit(UnitController unit)
    {
        SelectedUnits.Remove(unit);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SectorController : MonoBehaviour
{
    private HashSet<UnitController> _units = new();
    public List<TeleportPoint> Teleporters { get; private set; }

    public event Action<SectorController, int> OnSectorPowerChanged = delegate { };

    public int GetSectorPower() => _units.Sum(CalculateSectorPower);

    private int CalculateSectorPower(UnitController unit)
    {
        return 1;
    }

    public void AddUnit(UnitController unit)
    {
        _units.Add(unit);
        unit.SetSector(this);
        OnSectorPowerChanged.Invoke(this, GetSectorPower());
    }

    public void RemoveUnit(UnitController unit)
    {
        _units.Remove(unit);
        OnSectorPowerChanged.Invoke(this, GetSectorPower());
    }
}

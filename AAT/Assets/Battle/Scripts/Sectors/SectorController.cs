using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class SectorController : MonoBehaviour
{
    private HashSet<UnitController> _units = new HashSet<UnitController>();

    public event Action<SectorController, int> OnSectorPowerChanged = delegate { };

    public int GetSectorPower()
    {
        int sectorPower = 0;

        foreach (var unit in _units)
        {
            //TODO: super duper complex power calculation
            sectorPower += 1;
        }

        return sectorPower;
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

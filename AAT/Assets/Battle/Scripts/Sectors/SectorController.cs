using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorController : MonoBehaviour
{
    private List<UnitController> units = new List<UnitController>();

    public event Action<float> OnSectorPowerChanged = delegate { };

    public float GetSectorPower()
    {
        int sectorPower = 0;

        foreach (var unit in units)
        {
            //super duper complex power calculation
            sectorPower += 1;
        }

        return sectorPower;
    }

    public void AddUnit(UnitController unit)
    {
        units.Add(unit);
        unit.SetSector(this);
        OnSectorPowerChanged.Invoke(GetSectorPower());
    }

    public void RemoveUnit(UnitController unit)
    {
        units.Remove(unit);
        OnSectorPowerChanged.Invoke(GetSectorPower());
    }
}

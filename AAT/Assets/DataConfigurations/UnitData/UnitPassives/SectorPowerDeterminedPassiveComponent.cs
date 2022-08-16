using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class SectorPowerDeterminedPassiveComponent : PassiveComponent
{
    [SerializeField] private List<int> sectorPowerThresholds;
    private Dictionary<SectorController, int> _currentThresholdIndexesBySector = new();
    protected Dictionary<SectorController, HashSet<UnitController>> _unitsBySector = new();

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        if (!_unitsBySector.ContainsKey(unit.Sector))
        {
            _unitsBySector[unit.Sector] = new HashSet<UnitController>();
            unit.Sector.OnSectorPowerChanged += RedetermineThreshold;
        }
        
        _unitsBySector[unit.Sector].Add(unit);
        
        var thresholdIndex = DetermineThreshold(unit.Sector.SectorPower);
        if (thresholdIndex > -1)
        {
            _currentThresholdIndexesBySector[unit.Sector] = thresholdIndex;
            ActivateThresholdIndex(unit.Sector, thresholdIndex);
        }
    }

    public override void DeactivateComponent(UnitController unit)
    {
        _unitsBySector[unit.Sector].Remove(unit);
    }

    private int DetermineThreshold(float sectorPower)
    {
        for (int i = sectorPowerThresholds.Count - 1; i >= 0; i--)
        {
            if (sectorPower >= sectorPowerThresholds[i]) return i;
        }
        return -1;
    }

    private void RedetermineThreshold(SectorController sector)
    {
        var newPower = sector.SectorPower;
        
        if (_currentThresholdIndexesBySector.ContainsKey(sector))
        {
            if (newPower == sectorPowerThresholds[_currentThresholdIndexesBySector[sector]]) return;
            DeactivateThresholdIndex(sector, _currentThresholdIndexesBySector[sector]);
        }
        var thresholdIndex = DetermineThreshold(newPower);
        if (thresholdIndex > -1)
        {
            ActivateThresholdIndex(sector, thresholdIndex);
            _currentThresholdIndexesBySector[sector] = thresholdIndex;
        }
        else _currentThresholdIndexesBySector.Remove(sector);
    }

    protected abstract void ActivateThresholdIndex(SectorController sector, int index);

    protected abstract void DeactivateThresholdIndex(SectorController sector, int index);
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public abstract class SectorPowerDeterminedPassiveComponent : PassiveComponent
{
    [SerializeField] private List<int> sectorPowerThresholds;
    private Dictionary<SectorController, int> _currentThresholdIndexesBySector = new Dictionary<SectorController, int>();
    protected Dictionary<SectorController, HashSet<UnitController>> _unitsBySector =
        new Dictionary<SectorController, HashSet<UnitController>>();

    public override void ActivateComponent(UnitController unit)
    {
        if (!_unitsBySector.ContainsKey(unit.SectorController))
        {
            _unitsBySector[unit.SectorController] = new HashSet<UnitController>();
            unit.SectorController.OnSectorPowerChanged += RedetermineThreshold;
        }
        
        _unitsBySector[unit.SectorController].Add(unit);
        
        var thresholdIndex = DetermineThreshold(unit.SectorController.GetSectorPower());
        if (thresholdIndex > -1)
        {
            _currentThresholdIndexesBySector[unit.SectorController] = thresholdIndex;
            ActivateThresholdIndex(unit.SectorController, thresholdIndex);
        }
    }

    private int DetermineThreshold(float sectorPower)
    {
        for (int i = sectorPowerThresholds.Count - 1; i >= 0; i--)
        {
            if (sectorPower >= sectorPowerThresholds[i]) return i;
        }
        return -1;
    }

    private void RedetermineThreshold(SectorController sector, int newPower)
    {
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

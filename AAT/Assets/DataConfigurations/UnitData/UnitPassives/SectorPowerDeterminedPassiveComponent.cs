using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SectorPowerDeterminedPassiveComponent : PassiveComponent
{
    [SerializeField] private List<float> sectorPowerThresholds;
    private int _currentThresholdIndex;
    protected UnitController _unit;

    public override void ActivateComponent(UnitController unitController)
    {
        _unit = unitController;
        var thresholdIndex = DetermineThreshold(unitController.SectorController.GetSectorPower());
        if (thresholdIndex > -1)
        {
            _currentThresholdIndex = thresholdIndex;
            ActivateThresholdIndex(thresholdIndex);
        }
        unitController.SectorController.OnSectorPowerChanged += RedetermineThreshold;
    }

    private int DetermineThreshold(float sectorPower)
    {
        for (int i = sectorPowerThresholds.Count - 1; i >= 0; i--)
        {
            if (sectorPower >= sectorPowerThresholds[i]) return i;
        }
        return -1;
    }

    private void RedetermineThreshold(float newPower)
    {
        if (_currentThresholdIndex == sectorPowerThresholds.Count - 1)
        {
            if (newPower < sectorPowerThresholds[_currentThresholdIndex])
            {
                DeactivateThresholdIndex(_currentThresholdIndex);
                var thresholdIndex = DetermineThreshold(newPower);
                if (thresholdIndex > -1) ActivateThresholdIndex(thresholdIndex);
            }
        }
        else if (newPower > sectorPowerThresholds[_currentThresholdIndex + 1] || newPower < sectorPowerThresholds[_currentThresholdIndex - 1])
        {
            DeactivateThresholdIndex(_currentThresholdIndex);
            var thresholdIndex = DetermineThreshold(newPower);
            if (thresholdIndex > -1) ActivateThresholdIndex(thresholdIndex);
        }
    }

    protected abstract void ActivateThresholdIndex(int index);

    protected abstract void DeactivateThresholdIndex(int index);
}

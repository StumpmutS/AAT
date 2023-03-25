using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class SectorPowerDeterminedPassiveGameAction : PassiveComponentState
{
    [SerializeField] private List<int> sectorPowerThresholds;

    private SectorReference _sectorReference;
    private TeamController _team;

    private int _currentThresholdIndex;
    private int CurrentThresholdIndex
    {
        get => _currentThresholdIndex;
        set
        {
            if (_currentThresholdIndex == value) return;
            
            DeactivateThresholdIndex(_sectorReference.Sector, _currentThresholdIndex);
            _currentThresholdIndex = value;
            ActivateThresholdIndex(_sectorReference.Sector, _currentThresholdIndex);
        }
    }

    protected abstract void ActivateThresholdIndex(SectorController sector, int index);

    protected abstract void DeactivateThresholdIndex(SectorController sector, int index);
    
    protected override void OnSpawnSuccess()
    {
        _team = GetComponent<TeamController>();
        _sectorReference = Container.GetComponent<SectorReference>();
        _sectorReference.OnSectorChanged += DetermineThreshold;
    }

    protected override void OnEnter()
    {
        DetermineThreshold();
    }

    private void DetermineThreshold()
    {
        for (int i = 0; i < sectorPowerThresholds.Count; i++)
        {
            if (_sectorReference.Sector.TeamPowers[_team.GetTeamNumber()] > sectorPowerThresholds[i])
            {
                CurrentThresholdIndex = i;
            }
        }
    }

    protected override void Tick() { }

    public override void OnExit() { }

    private void OnDestroy()
    {
        if (_sectorReference != null) _sectorReference.OnSectorChanged -= DetermineThreshold;
    }
}

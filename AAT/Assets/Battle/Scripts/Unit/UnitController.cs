using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : OutlineEntityController
{
    [SerializeField] private UnitStatsModifierManager unitStatsModifierManager;
    [SerializeField] private UnitDeathController unitDeathController;
    [SerializeField] private PoolingObject unitVisuals;     
    public PoolingObject UnitVisuals => unitVisuals;
    [SerializeField] private bool chaseState;
    public bool ChaseState => chaseState;
    [SerializeField] private bool patrolState;
    public bool PatrolState => patrolState;
    [SerializeField] private List<Vector3> _patrolPoints;
    public List<Vector3> PatrolPoints => _patrolPoints;

    private UnitGroupController unitGroup;
    public UnitGroupController UnitGroup => unitGroup;
    private SectorController sectorController;
    public SectorController SectorController => sectorController;

    public event Action<UnitController> OnDeath = delegate { };

    private void Awake()
    {
        unitDeathController.OnUnitDeath += UnitDeath;
    }

    private void UnitDeath()
    {
        Deselect();
        if (sectorController != null) sectorController.RemoveUnit(this);
        OnDeath.Invoke(this);
    }

    public void ModifyStats(UnitStatsDataInfo unitStatsDataInfo, bool add = true)
    {
        unitStatsModifierManager.ModifyStats(unitStatsDataInfo, add);
    }

    #region Setters
    public void SetSector(SectorController sector)
    {
        sectorController = sector;
    }

    public void SetGroup(UnitGroupController group)
    {
        unitGroup = group;
    }

    public void SetPatrolPoints(List<Vector3> patrolPoints)
    {
        _patrolPoints = patrolPoints;
        patrolState = true;
    }

    public void SetChaseState(bool value)
    {
        chaseState = value;
    }
    #endregion
}

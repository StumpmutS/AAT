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

    private AIPathfinder AI;
    private UnitGroupController unitGroup;
    public UnitGroupController UnitGroup => unitGroup;

    public event Action<UnitController> OnDeath = delegate { };

    private void Awake()
    {
        AI = GetComponent<AIPathfinder>();
        unitDeathController.OnUnitDeath += UnitDeath;
    }

    private void UnitDeath()
    {
        Deselect();
        OnDeath.Invoke(this);
    }

    public void ModifyStats(UnitStatsDataInfo unitStatsDataInfo)
    {
        unitStatsModifierManager.ModifyStats(unitStatsDataInfo);
    }

    public void SetGroup(UnitGroupController group)
    {
        unitGroup = group;
    }

    public void SetPatrolPoints(List<Vector3> patrolPoints)
    {
        AI.SetPatrolPoints(patrolPoints);
    }
}

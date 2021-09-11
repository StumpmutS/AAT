using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : EntityController
{
    [SerializeField] private UnitStatsModifierManager unitStatsModifierManager;
    [SerializeField] private UnitDeathController unitDeathController;
    [SerializeField] private GameObject unitVisuals;
    public GameObject UnitVisuals => unitVisuals;

    public event Action<UnitController> OnDeath = delegate { };

    private void Start()
    {
        unitDeathController.OnUnitDeath += UnitDeath;
    }

    private void UnitDeath()
    {
        OnDeath.Invoke(this);
    }

    public void ModifyStats(UnitStatsDataInfo unitStatsDataInfo)
    {
        unitStatsModifierManager.ModifyStats(unitStatsDataInfo);
    }
}

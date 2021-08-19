using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : EntityController
{
    [SerializeField] private UnitStatsModifierManager unitStatsModifierManager;
    [SerializeField] private UnitDeathController unitDeathController;

    public event Action<UnitController> OnDeath = delegate { };

    private void Start()
    {
        unitDeathController.OnUnitDeath += UnitDeath;
    }

    private void UnitDeath()
    {
        OnDeath.Invoke(this);
    }

    public void ModifyStats(List<EStat> stats = null, List<float> amounts = null, ETransportationType transportationType = ETransportationType.None, EAttackType attackType = EAttackType.None, EMovementType movementType = EMovementType.None)
    {
        unitStatsModifierManager.ModifyStats(stats, amounts, transportationType, attackType, movementType);
    }
}

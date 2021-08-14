using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : EntityController
{
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
}

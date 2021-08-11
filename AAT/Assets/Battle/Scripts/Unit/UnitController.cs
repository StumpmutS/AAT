using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : EntityController
{
    [SerializeField] UnitDeathController unitDeathController;

    public void SetupDeath(Action<int, int> deathCallback, int groupIndex, int unitIndex)
    {
        unitDeathController.Setup(deathCallback, groupIndex, unitIndex);
    }
}

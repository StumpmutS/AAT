using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Stats/Base Unit Stats Data")]
public class BaseUnitStatsData : ArmoredHealthUnitStatsData
{    
    public SerializableDictionary<EUnitFloatStats, float> UnitFloatStats = 
        new SerializableDictionary<EUnitFloatStats, float>()
        {
            {EUnitFloatStats.Damage, 0f },
            {EUnitFloatStats.CritMultiplierPercent, 0f },
            {EUnitFloatStats.CritChancePercent, 0f },
            {EUnitFloatStats.AttackSpeedPercent, 0f },
            {EUnitFloatStats.MovementSpeed, 0f },
            {EUnitFloatStats.SightRange, 0f },
            {EUnitFloatStats.AttackRange, 0f },
            {EUnitFloatStats.ChaseSpeedPercentMultiplier, 0f }
        };

    protected override void FillReturnStats()
    {
        base.FillReturnStats();
        foreach (var kvp in UnitFloatStats)
        {
            ReturnStats.Add(kvp.Key, kvp.Value);
        }
    }
}
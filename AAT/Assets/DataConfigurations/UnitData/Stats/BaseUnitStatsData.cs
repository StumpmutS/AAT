using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Stats/Base Unit Stats Data")]
public class BaseUnitStatsData : ArmoredHealthUnitStatsData
{    
    public SerializableDictionary<EUnitFloatStats, float> UnitFloatStats = new SerializableDictionary<EUnitFloatStats, float>()
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
    public ETransportationType TransportState;
    public EAttackType AttackState;
    public EMovementType MoveState;

    public override Dictionary<EUnitFloatStats, float> GetStats()
    {
        Dictionary<EUnitFloatStats, float> stats = new Dictionary<EUnitFloatStats, float>();
        foreach (var kvp in ArmoredHealthFloatStats)
        {
            stats.Add(kvp.Key, kvp.Value);
        }

        foreach (var kvp in UnitFloatStats)
        {
            stats.Add(kvp.Key, kvp.Value);
        }
        return stats;
    }
}
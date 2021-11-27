using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Stats/Unit Stats Data")]
public class BaseUnitStatsData : ScriptableObject
{    
    public SerializableDictionary<EUnitFloatStats, float> UnitFloatStats = new SerializableDictionary<EUnitFloatStats, float>()
    {
        {EUnitFloatStats.MaxHealth, 0f },
        {EUnitFloatStats.BaseArmorPercent, 0f },
        {EUnitFloatStats.MaxArmorPercent, 0f },
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

    public virtual Dictionary<EUnitFloatStats, float> GetStats()
    {
        return UnitFloatStats;
    }
}
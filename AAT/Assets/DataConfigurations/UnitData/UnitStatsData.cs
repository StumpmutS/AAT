using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Base/Unit Stats Data")]
public class UnitStatsData : ScriptableObject
{
    public UnitStatsDataInfo UnitStatsDataInfo;
}

[Serializable]
public class UnitStatsDataInfo
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
    public float MaxHealth;
    public float BaseArmorPercent;
    public float MaxArmorPercent;
    public float Damage;
    public float CritMultiplierPercent;
    public float CritChancePercent;
    public float AttackSpeedPercent;
    public float MovementSpeed;
    public float SightRange;
    public float AttackRange;
    public float ChaseSpeedPercentMultiplier;
    public ETransportationType TransportState;
    public EAttackType AttackState;
    public EMovementType MoveState;

    public void AddFloatStats(UnitStatsDataInfo stats)
    {
        foreach (var kvp in stats.UnitFloatStats)
        {
            UnitFloatStats[kvp.Key] += kvp.Value;
        }
    }

    public void SubtractFloatStats(UnitStatsDataInfo stats)
    {
        foreach (var kvp in stats.UnitFloatStats)
        {
            UnitFloatStats[kvp.Key] -= kvp.Value;
        }
    }

    public void AddSingle(EUnitFloatStats statType, float amount)
    {
        UnitFloatStats[statType] += amount;
    }
}

public enum EUnitFloatStats 
{ 
    MaxHealth,
    BaseArmorPercent,
    MaxArmorPercent,
    Damage,
    CritMultiplierPercent,
    CritChancePercent,
    AttackSpeedPercent,
    MovementSpeed,
    SightRange,
    AttackRange,
    ChaseSpeedPercentMultiplier,
}
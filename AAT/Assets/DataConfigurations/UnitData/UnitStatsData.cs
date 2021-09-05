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
}
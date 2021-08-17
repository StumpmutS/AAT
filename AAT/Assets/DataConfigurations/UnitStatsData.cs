using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Stats Data")]
public class UnitStatsData : ScriptableObject
{
    public float MaxHealth;
    public float BaseArmorPercent;
    public float MaxArmorPercent;
    public float Damage;
    public float AttackSpeedPercent;
    public float MovementSpeed;
    public float SightRange;
    public float AttackRange;
    public float ChaseSpeedPercentMultiplier;
    public ETransportationType TransportState;
    public EAttackType AttackState;
    public EMovementType MoveState;
}

public enum Stat
{
    None,
    MaxHealth,
    BaseArmorPercent,
    MaxArmorPercent,
    Damage,
    AttackSpeedPercent,
    MovementSpeed,
    SightRange,
    AttackRange,
    ChaseSpeedPercentMultiplier
};
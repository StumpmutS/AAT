using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data")]
public class UnitData : ScriptableObject
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
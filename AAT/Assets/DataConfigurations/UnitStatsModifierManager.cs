using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatsModifierManager : MonoBehaviour
{
    [SerializeField] private UnitStatsData baseUnitStatsData;

    private UnitStatsDataStruct currentUnitStatsData;
    public UnitStatsDataStruct CurrentUnitStatsData => currentUnitStatsData;

    public event Action OnRefreshStats = delegate{ };

    private void Awake()
    {
        if (baseUnitStatsData != null)
        {
            SetupCurrentUnitStatsData(baseUnitStatsData);
        }
    }

    public void Setup(UnitStatsData unitStatsData)
    {
        SetupCurrentUnitStatsData(unitStatsData);
    }
    
    private void SetupCurrentUnitStatsData(UnitStatsData unitStatsData)
    {
        currentUnitStatsData.MaxHealth = unitStatsData.MaxHealth;
        currentUnitStatsData.BaseArmorPercent = unitStatsData.BaseArmorPercent;
        currentUnitStatsData.MaxArmorPercent = unitStatsData.MaxArmorPercent;
        currentUnitStatsData.Damage = unitStatsData.Damage;
        currentUnitStatsData.AttackSpeedPercent = unitStatsData.AttackSpeedPercent;
        currentUnitStatsData.MovementSpeed = unitStatsData.MovementSpeed;
        currentUnitStatsData.SightRange = unitStatsData.SightRange;
        currentUnitStatsData.AttackRange = unitStatsData.AttackRange;
        currentUnitStatsData.ChaseSpeedPercentMultiplier = unitStatsData.ChaseSpeedPercentMultiplier;
        currentUnitStatsData.TransportState = unitStatsData.TransportState;
        currentUnitStatsData.AttackState = unitStatsData.AttackState;
        currentUnitStatsData.MoveState = unitStatsData.MoveState;
    }

    public void ModifyStats(List<EStat> stats = null, List<float> amounts = null, ETransportationType transportationType = ETransportationType.None, EAttackType attackType = EAttackType.None, EMovementType movementType = EMovementType.None)
    {
        if (stats != null && stats.Count > 0 && amounts != null && amounts.Count > 0)
        {
            for (int i = 0; i < stats.Count; i++)
            {
                switch (stats[i])
                {
                    case EStat.None:
                        break;
                    case EStat.MaxHealth:
                        currentUnitStatsData.MaxHealth += amounts[i];
                        break;
                    case EStat.BaseArmorPercent:
                        currentUnitStatsData.BaseArmorPercent += amounts[i];
                        break;
                    case EStat.MaxArmorPercent:
                        currentUnitStatsData.MaxArmorPercent += amounts[i];
                        break;
                    case EStat.Damage:
                        currentUnitStatsData.Damage += amounts[i];
                        break;
                    case EStat.AttackSpeedPercent:
                        currentUnitStatsData.AttackSpeedPercent += amounts[i];
                        break;
                    case EStat.MovementSpeed:
                        currentUnitStatsData.MovementSpeed += amounts[i];
                        break;
                    case EStat.SightRange:
                        currentUnitStatsData.SightRange += amounts[i];
                        break;
                    case EStat.AttackRange:
                        currentUnitStatsData.AttackRange += amounts[i];
                        break;
                    case EStat.ChaseSpeedPercentMultiplier:
                        currentUnitStatsData.ChaseSpeedPercentMultiplier += amounts[i];
                        break;
                }
            }
        }

        if (transportationType != ETransportationType.None)
        {
            currentUnitStatsData.TransportState = transportationType;
        }
        if (attackType != EAttackType.None)
        {
            currentUnitStatsData.AttackState = attackType;
        }
        if (movementType != EMovementType.None)
        {
            currentUnitStatsData.MoveState = movementType;
        }

        OnRefreshStats.Invoke();
    }
}

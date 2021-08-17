using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatsUpgradeManager : MonoBehaviour
{
    [SerializeField] private UnitStatsData baseUnitStatsData;

    private UnitStatsData currentUnitStatsData;
    public UnitStatsData CurrentUnitStatsData => currentUnitStatsData;

    public event Action OnRefreshStats = delegate{ };

    private void Awake()
    {
        currentUnitStatsData = baseUnitStatsData;
    }

    public void ModifyStats(List<Stat> stats, List<float> amounts)
    {
        for (int i = 0; i < stats.Count; i++)
        {
            switch (stats[i])
            {
                case Stat.None:
                    break;
                case Stat.MaxHealth:
                    currentUnitStatsData.MaxHealth += amounts[i];
                    break;
                case Stat.BaseArmorPercent:
                    currentUnitStatsData.BaseArmorPercent += amounts[i];
                    break;
                case Stat.MaxArmorPercent:
                    currentUnitStatsData.MaxArmorPercent += amounts[i];
                    break;
                case Stat.Damage:
                    currentUnitStatsData.Damage += amounts[i];
                    break;
                case Stat.AttackSpeedPercent:
                    currentUnitStatsData.AttackSpeedPercent += amounts[i];
                    break;
                case Stat.MovementSpeed:
                    currentUnitStatsData.MovementSpeed += amounts[i];
                    break;
                case Stat.SightRange:
                    currentUnitStatsData.SightRange += amounts[i];
                    break;
                case Stat.AttackRange:
                    currentUnitStatsData.AttackRange += amounts[i];
                    break;
                case Stat.ChaseSpeedPercentMultiplier:
                    currentUnitStatsData.ChaseSpeedPercentMultiplier += amounts[i];
                    break;
            }
        }

        OnRefreshStats.Invoke();
    }

    public void ModifyTransportState(ETransportationType transportationType)
    {
        currentUnitStatsData.TransportState = transportationType;
        OnRefreshStats.Invoke();
    }

    public void ModifyAttackState(EAttackType attackType)
    {
        currentUnitStatsData.AttackState = attackType;
        OnRefreshStats.Invoke();
    }

    public void ModifyMovementState(EMovementType movementType)
    {
        currentUnitStatsData.MoveState = movementType;
        OnRefreshStats.Invoke();
    }
}

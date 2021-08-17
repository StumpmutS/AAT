using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatsModifierManager : MonoBehaviour
{
    [SerializeField] private UnitStatsData baseUnitStatsData;

    private UnitStatsData currentUnitStatsData;
    public UnitStatsData CurrentUnitStatsData => currentUnitStatsData;

    public event Action OnRefreshStats = delegate{ };

    private void Awake()
    {
        currentUnitStatsData = baseUnitStatsData;
    }

    public void ModifyStats(List<Stat> stats = null, List<float> amounts = null, ETransportationType transportationType = ETransportationType.None, EAttackType attackType = EAttackType.None, EMovementType movementType = EMovementType.None)
    {
        if (stats != null && amounts != null)
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

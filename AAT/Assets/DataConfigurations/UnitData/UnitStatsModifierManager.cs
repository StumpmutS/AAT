using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatsModifierManager : MonoBehaviour
{
    [SerializeField] private UnitStatsData baseUnitStatsData;

    private UnitStatsDataInfo currentUnitStatsData;
    public UnitStatsDataInfo CurrentUnitStatsData => currentUnitStatsData;

    public event Action OnRefreshStats = delegate { };

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
        var instantiatedUnitStatsData = Instantiate(unitStatsData);
        currentUnitStatsData = instantiatedUnitStatsData.UnitStatsDataInfo;
    }

    public void ModifyStats(UnitStatsDataInfo unitStatsDataInfo)
    {
        currentUnitStatsData.MaxHealth += unitStatsDataInfo.MaxHealth;
        currentUnitStatsData.BaseArmorPercent += unitStatsDataInfo.BaseArmorPercent;
        currentUnitStatsData.MaxArmorPercent += unitStatsDataInfo.MaxArmorPercent;
        currentUnitStatsData.Damage += unitStatsDataInfo.Damage;
        currentUnitStatsData.CritMultiplierPercent += unitStatsDataInfo.CritMultiplierPercent;
        currentUnitStatsData.CritChancePercent += unitStatsDataInfo.CritChancePercent;
        currentUnitStatsData.AttackSpeedPercent += unitStatsDataInfo.AttackSpeedPercent;
        currentUnitStatsData.MovementSpeed += unitStatsDataInfo.MovementSpeed;
        currentUnitStatsData.SightRange += unitStatsDataInfo.SightRange;
        currentUnitStatsData.AttackRange += unitStatsDataInfo.AttackRange;
        currentUnitStatsData.ChaseSpeedPercentMultiplier += unitStatsDataInfo.ChaseSpeedPercentMultiplier;

        if (unitStatsDataInfo.TransportState != ETransportationType.None)
        {
            currentUnitStatsData.TransportState = unitStatsDataInfo.TransportState;
        }
        if (unitStatsDataInfo.AttackState != EAttackType.None)
        {
            currentUnitStatsData.AttackState = unitStatsDataInfo.AttackState;
        }
        if (unitStatsDataInfo.MoveState != EMovementType.None)
        {
            currentUnitStatsData.MoveState = unitStatsDataInfo.MoveState;
        }

        OnRefreshStats.Invoke();
    }
}

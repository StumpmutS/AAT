using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitStatsModifierManager : MonoBehaviour
{
    [SerializeField] private BaseUnitStatsData unitStatsData;

    public Dictionary<EUnitFloatStats, float> CurrentUnitStatsData { get; private set; } =
        new Dictionary<EUnitFloatStats, float>();
    public ETransportationType TransportationType { get; private set; }
    public EAttackType AttackType { get; private set; }
    public EMovementType MovementType { get; private set; }
    public event Action OnRefreshStats = delegate { };

    private void Awake()
    {
        if (unitStatsData != null)
        {
            SetupCurrentUnitStatsData(unitStatsData);
        }
    }

    public void Setup(BaseUnitStatsData unitStatsData)
    {
        SetupCurrentUnitStatsData(unitStatsData);
    }
    
    private void SetupCurrentUnitStatsData(BaseUnitStatsData unitStatsData)
    {
        foreach (var kvp in unitStatsData.UnitFloatStats)
        {
            CurrentUnitStatsData[kvp.Key] = kvp.Value;
        }
    }

    public void ModifyStats(BaseUnitStatsData baseUnitStatsDataInfo, bool add = true)
    {
        if (add) AddFloatStats(baseUnitStatsDataInfo);
        else SubtractFloatStats(baseUnitStatsDataInfo);

        OnRefreshStats.Invoke();
    }

    public void ModifyTransportationType(ETransportationType transportationType)
    {
        TransportationType = transportationType;
        OnRefreshStats.Invoke();
    }

    public void ModifyAttackType(EAttackType attackType)
    {
        AttackType = attackType;
        OnRefreshStats.Invoke();
    }

    public void ModifyMovementType(EMovementType movementType)
    {
        MovementType = movementType;
        OnRefreshStats.Invoke();
    }
    
    private void AddFloatStats(BaseUnitStatsData stats)
    {
        foreach (var kvp in stats.UnitFloatStats)
        {
            CurrentUnitStatsData[kvp.Key] += kvp.Value;
        }
    }

    private void SubtractFloatStats(BaseUnitStatsData stats)
    {
        foreach (var kvp in stats.UnitFloatStats)
        {
            CurrentUnitStatsData[kvp.Key] -= kvp.Value;
        }
    }

    public void ModifyFloatStat(EUnitFloatStats statType, float amount)
    {
        CurrentUnitStatsData[statType] += amount;
    }
}

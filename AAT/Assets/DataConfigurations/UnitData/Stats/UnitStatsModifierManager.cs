using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitStatsModifierManager : MonoBehaviour
{
    [SerializeField] private ArmoredHealthUnitStatsData unitStatsData;

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

    public void Setup(ArmoredHealthUnitStatsData unitStatsData)
    {
        SetupCurrentUnitStatsData(unitStatsData);
    }
    
    private void SetupCurrentUnitStatsData(ArmoredHealthUnitStatsData unitStatsData)
    {
        foreach (var kvp in unitStatsData.GetStats())
        {
            CurrentUnitStatsData[kvp.Key] = kvp.Value;
        }
    }

    public void ModifyStats(ArmoredHealthUnitStatsData stats, bool add = true)
    {
        if (add) AddFloatStats(stats);
        else SubtractFloatStats(stats);

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
    
    private void AddFloatStats(ArmoredHealthUnitStatsData stats)
    {
        foreach (var stat in stats.GetStats().Where(kvp => CurrentUnitStatsData.ContainsKey(kvp.Key)))
        {
            CurrentUnitStatsData[stat.Key] += stat.Value;
        }
    }

    private void SubtractFloatStats(ArmoredHealthUnitStatsData stats)
    {
        foreach (var stat in stats.GetStats().Where(kvp => CurrentUnitStatsData.ContainsKey(kvp.Key)))
        {
            CurrentUnitStatsData[stat.Key] -= stat.Value;
        }
    }

    public void ModifyFloatStat(EUnitFloatStats statType, float amount)
    {
        if (CurrentUnitStatsData.ContainsKey(statType))
            CurrentUnitStatsData[statType] += amount;
    }
}

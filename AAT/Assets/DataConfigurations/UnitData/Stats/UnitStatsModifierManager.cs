using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            Setup(unitStatsData);
        }
    }

    public void Setup(BaseUnitStatsData unitStatsData)
    {
        foreach (var stat in unitStatsData.Stats)
        {
            CurrentUnitStatsData[stat.Stat] = stat.Value;
        }
    }

    public void ModifyStats(BaseUnitStatsData stats, bool add = true)
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
    
    private void AddFloatStats(BaseUnitStatsData stats)
    {
        foreach (var stat in stats.Stats.Where(stat => CurrentUnitStatsData.ContainsKey(stat.Stat)))
        {
            CurrentUnitStatsData[stat.Stat] += stat.Value;
        }
    }

    private void SubtractFloatStats(BaseUnitStatsData stats)
    {
        foreach (var stat in stats.Stats.Where(stat => CurrentUnitStatsData.ContainsKey(stat.Stat)))
        {
            CurrentUnitStatsData[stat.Stat] -= stat.Value;
        }
    }

    public void ModifyFloatStat(EUnitFloatStats statType, float amount)
    {
        if (CurrentUnitStatsData.ContainsKey(statType))
            CurrentUnitStatsData[statType] += amount;
    }
}

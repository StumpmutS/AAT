using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitStatsModifierManager : MonoBehaviour
{
    [SerializeField] private BaseUnitStatsData unitStatsData;

    public Dictionary<EUnitFloatStats, float> CurrentStats { get; } =
        new();
    
    public event Action OnRefreshStats = delegate { };

    private void Awake()
    {
        if (unitStatsData != null)
        {
            Init(unitStatsData);
        }
    }

    public void Init(BaseUnitStatsData unitStatsData)
    {
        foreach (var stat in unitStatsData.Stats)
        {
            CurrentStats[stat.Stat] = stat.Value;
        }
    }

    public void ModifyStats(BaseUnitStatsData stats, bool add = true)
    {
        if (add) AddFloatStats(stats);
        else SubtractFloatStats(stats);

        OnRefreshStats.Invoke();
    }

    private void AddFloatStats(BaseUnitStatsData stats)
    {
        foreach (var stat in stats.Stats.Where(stat => CurrentStats.ContainsKey(stat.Stat)))
        {
            CurrentStats[stat.Stat] += stat.Value;
        }
    }

    private void SubtractFloatStats(BaseUnitStatsData stats)
    {
        foreach (var stat in stats.Stats.Where(stat => CurrentStats.ContainsKey(stat.Stat)))
        {
            CurrentStats[stat.Stat] -= stat.Value;
        }
    }

    public void ModifyFloatStat(EUnitFloatStats statType, float amount)
    {
        if (!CurrentStats.ContainsKey(statType)) return;
        CurrentStats[statType] += amount;
        OnRefreshStats.Invoke();
    }
}

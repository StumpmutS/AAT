using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class UnitStatsModifierManager : NetworkBehaviour
{
    [SerializeField] private BaseUnitStatsData unitStatsData;

    [Networked(nameof(OnRefreshStats)), Capacity(32)] //TODO: test if could invoke refreshStats onchanged?
    private NetworkDictionary<EUnitFloatStats, float> _currentStats => default;

    private Dictionary<EUnitFloatStats, float> _startingStats = new();

    public event Action OnRefreshStats = delegate { };

    private void Awake()
    {
        if (unitStatsData == null) return;
        
        foreach (var stat in unitStatsData.Stats)
        {
            _startingStats[stat.Stat] = stat.Value;
        }
    }

    private void Start()
    {
        if (unitStatsData == null) return;
        Init(unitStatsData);
    }

    public void Init(BaseUnitStatsData givenUnitStatsData)
    {
        foreach (var stat in givenUnitStatsData.Stats)
        {
            _currentStats.Set(stat.Stat, stat.Value);
        }
    }
    
    public float GetStat(EUnitFloatStats stat)
    {
        if (_currentStats.TryGet(stat, out var value)) return value;

        Debug.LogWarning($"Stat type: {stat} could not be found in the networked dictionary");
        if (_startingStats.TryGetValue(stat, out value)) return value;
        
        Debug.LogError($"Stat type: {stat} could not be found in either networked or starting stat dictionaries");
        return value;
    }

    public void SetStat(EUnitFloatStats stat, float value) => _currentStats.Set(stat, value);

    public void ModifyStats(BaseUnitStatsData stats, bool add = true)
    {
        if (add) AddFloatStats(stats);
        else SubtractFloatStats(stats);

        //OnRefreshStats.Invoke();
    }

    private void AddFloatStats(BaseUnitStatsData stats)
    {
        foreach (var stat in stats.Stats.Where(stat => _currentStats.ContainsKey(stat.Stat)))
        {
            _currentStats.Set(stat.Stat, GetStat(stat.Stat) + stat.Value);
        }
    }

    private void SubtractFloatStats(BaseUnitStatsData stats)
    {
        foreach (var stat in stats.Stats.Where(stat => _currentStats.ContainsKey(stat.Stat)))
        {
            _currentStats.Set(stat.Stat, GetStat(stat.Stat) - stat.Value);
        }
    }

    public void ModifyFloatStat(EUnitFloatStats statType, float amount)
    {
        if (!_currentStats.ContainsKey(statType)) return;
        _currentStats.Set(statType, GetStat(statType) + amount);
        //OnRefreshStats.Invoke();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class StatsManager : NetworkBehaviour
{
    [Networked(nameof(OnChange)), Capacity(32)]
    private NetworkDictionary<EUnitFloatStats, float> _currentStats => default;
    private void OnChange(Changed<StatsManager> changed)
    {
        changed.Behaviour.OnRefreshStats.Invoke(changed.Behaviour);
    }
    
    [SerializeField] private BaseUnitStatsData unitStatsData;

    private Dictionary<EUnitFloatStats, float> _startingStats = new();
    private List<StatModifier> _modifiers;

    public event Action<StatsManager> OnRefreshStats = delegate { };

    private void Awake()
    {
        if (unitStatsData == null) return;
        
        foreach (var stat in unitStatsData.Stats)
        {
            _startingStats[stat.Stat] = stat.Value;
        }
    }

    public override void Spawned()
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
        if (_currentStats.TryGet(stat, out var value))
        {
            
        }
        else if (_startingStats.TryGetValue(stat, out value))
        {
            Debug.LogWarning($"Stat type: {stat} could not be found in the networked dictionary");
        }
        else
        {
            Debug.LogError($"Stat type: {stat} could not be found in either networked or starting stat dictionaries");
        }
        
        return value;
    }

    public void SetStat(EUnitFloatStats stat, float value) => _currentStats.Set(stat, value);

    public void ModifyStats(BaseUnitStatsData stats, bool add = true)
    {
        if (add) AddFloatStats(stats);
        else SubtractFloatStats(stats);
    }

    private void AddFloatStats(BaseUnitStatsData stats)
    {
        foreach (var stat in stats.Stats.Where(stat => _currentStats.ContainsKey(stat.Stat)))
        {
            SetStat(stat.Stat, GetStat(stat.Stat) + stat.Value);
        }
    }

    private void SubtractFloatStats(BaseUnitStatsData stats)
    {
        foreach (var stat in stats.Stats.Where(stat => _currentStats.ContainsKey(stat.Stat)))
        {
            SetStat(stat.Stat, GetStat(stat.Stat) - stat.Value);
        }
    }

    public void ModifyFloatStat(EUnitFloatStats statType, float amount)
    {
        if (!_currentStats.ContainsKey(statType)) return;
        _currentStats.Set(statType, GetStat(statType) + amount);
    }

    public int CalculatePower()
    {
        return 1;
    }

    public void AddModifier(StatModifier modifier)
    {
        Dictionary<EUnitFloatStats, float> fakeStats = new();
        foreach (var kvp in _currentStats)
        {
            fakeStats[kvp.Key] = kvp.Value;
        }
        fakeStats = modifier.ApplyModifierTo(fakeStats);
        foreach (var kvp in fakeStats)
        {
            SetStat(kvp.Key, kvp.Value);
        }
        
        _modifiers.Add(modifier);
    }

    public void RemoveModifier(StatModifier modifier)
    {
        Dictionary<EUnitFloatStats, float> fakeStats = new();
        foreach (var kvp in _currentStats)
        {
            fakeStats[kvp.Key] = kvp.Value;
        }
        fakeStats = modifier.RemoveModifierFrom(fakeStats);
        foreach (var kvp in fakeStats)
        {
            SetStat(kvp.Key, kvp.Value);
        }

        _modifiers.Remove(modifier);
    }
}

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
    [SerializeField] private EffectContainer effectContainer;

    private Dictionary<EUnitFloatStats, float> _startingStats = new();

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

    private float GetBaseStat(EUnitFloatStats stat)
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
    
    public float GetModifiedStat(EUnitFloatStats stat)
    {
        var value = GetBaseStat(stat);

        foreach (var modifier in effectContainer.GetEffects<StatModifier>())
        {
            value = modifier.ApplyModifierTo(stat, value);
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
            SetStat(stat.Stat, GetBaseStat(stat.Stat) + stat.Value);
        }
    }

    private void SubtractFloatStats(BaseUnitStatsData stats)
    {
        foreach (var stat in stats.Stats.Where(stat => _currentStats.ContainsKey(stat.Stat)))
        {
            SetStat(stat.Stat, GetBaseStat(stat.Stat) - stat.Value);
        }
    }

    public void ModifyFloatStat(EUnitFloatStats statType, float amount)
    {
        if (!_currentStats.ContainsKey(statType)) return;
        _currentStats.Set(statType, GetModifiedStat(statType) + amount);
    }

    public int CalculatePower()
    {
        return 1;
    }
}

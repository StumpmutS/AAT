using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Stats/Cavalry Unit Stats Data")]
public class CavalryUnitStatsData : BaseUnitStatsData
{
    public SerializableDictionary<EUnitFloatStats, float> CavalryFloatStats =
        new SerializableDictionary<EUnitFloatStats, float>()
        {
            {EUnitFloatStats.ChargeEndDistance, 0f},
            {EUnitFloatStats.ReturnSpeed, 0f}
        };

    public override Dictionary<EUnitFloatStats, float> GetStats()
    {
        Dictionary<EUnitFloatStats, float> stats = new Dictionary<EUnitFloatStats, float>();
        foreach (var kvp in ArmoredHealthFloatStats)
        {
            stats.Add(kvp.Key, kvp.Value);
        }
        
        foreach (var kvp in UnitFloatStats)
        {
            stats.Add(kvp.Key, kvp.Value);
        }

        foreach (var kvp in CavalryFloatStats)
        {
            stats.Add(kvp.Key, kvp.Value);
        }
        return stats;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Stats/Unit Armored Health Stats Data")]
public class ArmoredHealthUnitStatsData : ScriptableObject
{
    public SerializableDictionary<EUnitFloatStats, float> ArmoredHealthFloatStats = 
        new SerializableDictionary<EUnitFloatStats, float>()
        {
            {EUnitFloatStats.MaxHealth, 0f },
            {EUnitFloatStats.BaseArmorPercent, 0f },
            {EUnitFloatStats.MaxArmorPercent, 0f }
        };

    protected Dictionary<EUnitFloatStats, float> ReturnStats = new Dictionary<EUnitFloatStats, float>();

    public Dictionary<EUnitFloatStats, float> GetStats()
    {
        ReturnStats.Clear();
        FillReturnStats();
        return ReturnStats;
    }

    protected virtual void FillReturnStats()
    {
        foreach (var kvp in ArmoredHealthFloatStats)
        {
            ReturnStats.Add(kvp.Key, kvp.Value);
        }
    }
}

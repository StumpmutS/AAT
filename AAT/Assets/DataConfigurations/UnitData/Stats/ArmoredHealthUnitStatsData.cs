using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Stats/Unit Armored Health Stats Data")]
public class ArmoredHealthUnitStatsData : ScriptableObject
{
    public SerializableDictionary<EUnitFloatStats, float> ArmoredHealthFloatStats = new SerializableDictionary<EUnitFloatStats, float>()
    {
        {EUnitFloatStats.MaxHealth, 0f },
        {EUnitFloatStats.BaseArmorPercent, 0f },
        {EUnitFloatStats.MaxArmorPercent, 0f }
    };

    public virtual Dictionary<EUnitFloatStats, float> GetStats()
    {
        return ArmoredHealthFloatStats;
    }
}

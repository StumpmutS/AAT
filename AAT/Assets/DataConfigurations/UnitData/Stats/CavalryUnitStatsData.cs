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

    protected override void FillReturnStats()
    {
        base.FillReturnStats();
        foreach (var kvp in CavalryFloatStats)
        {
            ReturnStats.Add(kvp.Key, kvp.Value);
        }
    }
}
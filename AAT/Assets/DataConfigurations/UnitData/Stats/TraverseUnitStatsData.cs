using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Stats/Traverse Unit Stats Data")]
public class TraverseUnitStatsData : BaseUnitStatsData
{
    public SerializableDictionary<EUnitFloatStats, float> TraverseStats = 
        new SerializableDictionary<EUnitFloatStats, float>()
        {
            {EUnitFloatStats.TraverseSpeed, 0f }
        };

    protected override void FillReturnStats()
    {
        base.FillReturnStats();
        foreach (var kvp in TraverseStats)
        {
            ReturnStats.Add(kvp.Key, kvp.Value);
        }
    }
}

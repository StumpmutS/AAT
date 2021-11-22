using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Base/Cavalry Unit Stats Data")]
public class CavalryUnitStatsData : BaseUnitStatsData
{
    public SerializableDictionary<ECavalryFloatStat, float> CavalryFloatStats =
        new SerializableDictionary<ECavalryFloatStat, float>()
        {
            {ECavalryFloatStat.ChargeEndDistance, 0f},
            {ECavalryFloatStat.ReturnSpeed, 0f}
        };
}

public enum ECavalryFloatStat
{
    ChargeEndDistance,
    ReturnSpeed
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class UnitService : StumpService
{
    [SerializeField] private List<UnitData> unitData;
    
#pragma warning disable 1998
    public override async Task<List<StumpData>> RequestData()
#pragma warning restore 1998
    {
        return new List<StumpData>(unitData);
    }
}

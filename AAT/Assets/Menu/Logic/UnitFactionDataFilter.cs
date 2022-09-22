using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitFactionDataFilter : DataFilter
{
    [SerializeField] private EFaction factionFilter;

    public override List<StumpData> FilterData(List<StumpData> stumpData)
    {
        var unitData = stumpData.Select(d => (UnitData) d);
        var filteredData = unitData.Where(u => u.GeneralUnitData.Faction == factionFilter);
        return filteredData.Select(u => (StumpData) u).ToList();
    }

    public void SetFactionFilter(EFaction faction)
    {
        factionFilter = faction;
    }
}
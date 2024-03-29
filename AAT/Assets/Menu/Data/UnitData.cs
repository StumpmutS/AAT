using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitData : StumpData
{
    [SerializeField] private GeneralUnitData generalUnitData;
    public GeneralUnitData GeneralUnitData => generalUnitData;
    [SerializeField] private BaseUnitStatsData unitStatsData;
    public BaseUnitStatsData UnitStatsData => unitStatsData;
    [SerializeField] private List<UpgradeData> unitUpgradeData;
    public List<UpgradeData> UnitUpgradeData => unitUpgradeData;
    [SerializeField] private UnitArtData unitArtData;
    public UnitArtData UnitArtData => unitArtData;
}
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Stats/Base Unit Stats Data")]
public class  BaseUnitStatsData : ScriptableObject
{
    public List<UnitStat> Stats = new();
}

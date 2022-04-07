using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Unit Data/Stats/Base Unit Stats Data")]
public class BaseUnitStatsData : ScriptableObject
{
    public List<UnitStat> Stats = new List<UnitStat>();
}

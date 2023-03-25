using System.Collections.Generic;
using UnityEngine;

public abstract class StatModifier : StumpEffect
{
    [SerializeField] protected BaseUnitStatsData stats;

    public abstract float ApplyModifierTo(EUnitFloatStats unitStat, float value);
}
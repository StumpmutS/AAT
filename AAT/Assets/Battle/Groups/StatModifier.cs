using System.Collections.Generic;
using UnityEngine;

public abstract class StatModifier : ScriptableObject
{
    [SerializeField] private BaseUnitStatsData stats;
    public BaseUnitStatsData Stats => stats;

    public abstract Dictionary<EUnitFloatStats, float> ApplyModifierTo(Dictionary<EUnitFloatStats, float> stats);
    public abstract Dictionary<EUnitFloatStats, float> RemoveModifierFrom(Dictionary<EUnitFloatStats, float> stats);
}
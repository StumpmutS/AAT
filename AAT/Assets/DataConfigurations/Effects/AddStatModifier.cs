using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Add Stat Mod")]
public class AddStatModifier : StatModifier
{
    public override float ApplyModifierTo(EUnitFloatStats statType, float value)
    {
        foreach (var unitStat in stats.Stats)
        {
            if (unitStat.Stat == statType)
            {
                return value + unitStat.Value;
            }
        }

        return value;
    }
}
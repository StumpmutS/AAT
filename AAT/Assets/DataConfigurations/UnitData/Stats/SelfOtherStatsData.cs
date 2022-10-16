using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Stats/SelfOther Unit Stats Data")]
public class SelfOtherStatsData : ScriptableObject
{
    public StatModifier SelfModifier;
    public StatModifier OtherModifier;
}

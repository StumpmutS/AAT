using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Stats/SelfOther Unit Stats Data")]
public class SelfOtherStatsData : ScriptableObject
{
    public BaseUnitStatsData SelfModifier;
    public BaseUnitStatsData OtherModifier;
}

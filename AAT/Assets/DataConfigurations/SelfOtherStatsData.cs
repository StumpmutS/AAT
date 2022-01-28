using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Stats/SelfOther Unit Stats Data")]
public class SelfOtherStatsData : ScriptableObject
{
    public ArmoredHealthUnitStatsData SelfModifier;
    public ArmoredHealthUnitStatsData OtherModifier;
}

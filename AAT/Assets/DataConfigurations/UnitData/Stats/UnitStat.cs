using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Stats/Stat")]
public class UnitStat : ScriptableObject
{
    public EUnitFloatStats Stat;
    public float Value;
}

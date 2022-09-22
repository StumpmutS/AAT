using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Unit Data/Stats/Stat Icons")]
public class UnitStatIcons : ScriptableObject
{
    public SerializableDictionary<EUnitFloatStats, List<StylizedTextImage>> StatIcons;
}

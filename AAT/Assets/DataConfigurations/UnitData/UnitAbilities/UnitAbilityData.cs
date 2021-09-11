using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Unit Ability Data")]
public class UnitAbilityData : ScriptableObject
{
    public UnitAbilityDataInfo unitAbilityDataInfo;
}

[Serializable]
public class UnitAbilityDataInfo
{
    public string AbilityName;
    public float CooldownTime;
    public float ActiveTime;
    public bool TransportUsage;
    public List<AbilityComponent> AbilityComponents;
}
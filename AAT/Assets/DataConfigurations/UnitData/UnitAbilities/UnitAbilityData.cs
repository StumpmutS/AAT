using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Unit Ability Data")]
public class UnitAbilityData : ScriptableObject
{
    public UnitAbilityDataInfo UnitAbilityDataInfo;
}

[Serializable]
public class UnitAbilityDataInfo
{
    public string AbilityName;
    public float CooldownTime;
    public float ActiveTime;
    public float TargetTimeOutTime;
    [FormerlySerializedAs("TransportUsage")] public bool InteractUsage;
    public bool CanBeCastOver;
    [ShowIf(nameof(CanBeCastOver), true)]
    public float AllowCastOverTimer;
    public List<Restriction> Restrictions;
    public List<AbilityComponent> AbilityComponents;
    public List<UnitVisualComponent> UnitVisualComponents;
    public List<VisualComponent> VisualComponents;
}
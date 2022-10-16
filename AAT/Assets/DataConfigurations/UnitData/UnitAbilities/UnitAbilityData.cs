using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Unit Ability Data")]
public class UnitAbilityData : ScriptableObject, IStatePrefabGen
{
    public UnitAbilityDataInfo UnitAbilityDataInfo;
    
    [ContextMenu("GenerateStatePrefabs")]
    public void GenerateStatePrefabs()
    {
        if (!PrefabGen.TryGenerateStateFolder(GetInstanceID(), out var path)) return;
        if (!PrefabGen.GenerateStatePrefab(UnitAbilityDataInfo.StateType.TargetType, path, "", out var prefab)) return;
        
        UnitAbilityDataInfo.StatePrefab = prefab.GetComponent<ComponentState<AiTransitionBlackboard>>();
    }
}

[Serializable]
public class UnitAbilityDataInfo
{
    public string AbilityName;
    public List<StylizedTextImage> Icon;
    public KeyCode KeyCode;
    public float CooldownTime;
    public float ActiveTime;
    public float TargetTimeOutTime;
    public bool InteractUsage;
    public bool CanBeCastOver;
    [ShowIf(nameof(CanBeCastOver), true)]
    public float AllowCastOverTimer;
    public List<Restriction> Restrictions;
    public TypeReference StateType;
    public ComponentState<AiTransitionBlackboard> StatePrefab;
    public List<AbilityComponent> AbilityComponents;
    public List<UnitVisualComponent> UnitVisualComponents;
    public List<VisualComponent> VisualComponents;
}
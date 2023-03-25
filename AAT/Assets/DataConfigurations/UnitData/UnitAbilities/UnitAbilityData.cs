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
        
        UnitAbilityDataInfo.StatePrefab = prefab.GetComponent<AbilityComponentState>();
    }
}

[Serializable]
public class UserActionInfo
{
    [SerializeField] private string label;
    public string Label => label;
    [SerializeField] private List<StylizedTextImage> icon;
    public List<StylizedTextImage> Icon => icon;
    [SerializeField] private KeyCode keyCode;
    public KeyCode KeyCode => keyCode;
}

[Serializable]
public class UnitAbilityDataInfo
{
    [SerializeField] private UserActionInfo userActionInfo;
    public UserActionInfo UserActionInfo => userActionInfo;
    [FormerlySerializedAs("groupRestrictions")] [SerializeField] private List<Restriction> restrictions;
    public List<Restriction> Restrictions => restrictions;
    public TypeReference StateType;
    public AbilityComponentState StatePrefab;
}
using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Unit Data/Base/Spawner Spawn Data")]
public class SpawnerSpawnData : ScriptableObject
{
    [SerializeField] private UserActionInfo userActionInfo;
    public UserActionInfo UserActionInfo => userActionInfo;
    [SerializeField] private EFaction faction;
    public EFaction Faction => faction;
    public GroupMemberSpawner SpawnerPrefab;
}
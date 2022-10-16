using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Base/Unit Spawn Data")]
public class UnitSpawnData : ScriptableObject
{
    public GroupSpawnerController SpawnerPrefab;
    public int SpawnGroupsAmount;
    public int UnitsPerGroupAmount;
    public float SpawnTime;
    public float RespawnTime;
    public Group UnitGroupPrefab;
    public BaseUnitStatsData baseUnitStatsData;
    public List<UnitUpgradeData> UnitStatsUpgradeData;
}

[Serializable]
public class UnitSpawnDataListList
{
    public List<UnitSpawnData> UnitSpawnDataList;
    public List<Vector3ListList> UnitPatrolPointLists;
}

[Serializable]
public class Vector3ListList
{
    public List<Vector3> Vector3s;
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Unit Data/Base/Unit Spawn Data")]
public class UnitSpawnData : ScriptableObject
{
    public int SpawnGroupsAmount;
    public int UnitsPerGroupAmount;
    public float SpawnTime;
    public float RespawnTime;
    public int MaxSpawnLocationUse;
    public Vector3 SpawnerOffset;
    public GameObject SpawnerVisuals;
    public UnitController UnitPrefab;
    public UnitGroupController UnitGroupPrefab;
    public BaseUnitStatsData baseUnitStatsData;
    public List<UnitStatsUpgradeData> UnitStatsUpgradeData;
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
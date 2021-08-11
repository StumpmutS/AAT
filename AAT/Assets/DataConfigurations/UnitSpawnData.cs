using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Spawn Data")]
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
}

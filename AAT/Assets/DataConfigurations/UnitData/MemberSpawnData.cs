using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Spawn Data/Member")]
public class MemberSpawnData : ScriptableObject
{
    [SerializeField] private MemberSpawnInfo info;
    public MemberSpawnInfo Info => info;
}

[Serializable]
public class MemberSpawnInfo
{
    [SerializeField] private GroupMember memberPrefab;
    public GroupMember MemberPrefab => memberPrefab;
    [SerializeField] private int unitsPerGroupAmount;
    public int UnitsPerGroupAmount => unitsPerGroupAmount;
    [SerializeField] private float spawnTime;
    public float SpawnTime => spawnTime;
}
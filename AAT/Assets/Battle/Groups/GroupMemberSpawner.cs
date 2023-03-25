using System.Collections.Generic;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(GroupFormatter), typeof(Group))]
public class GroupMemberSpawner : SimulationBehaviour
{
    [SerializeField] private MemberSpawnData memberSpawnData;
    [SerializeField] private GroupMemberSpawnPointController spawnPoint;

    private Group _group;
    private GroupFormatter _groupFormatter;

    private void Awake()
    {
        _group = GetComponent<Group>();
        _groupFormatter = GetComponent<GroupFormatter>();
    }

    private void Start()
    {
        if (memberSpawnData != null) SpawnMembers(memberSpawnData.Info.MemberPrefab, memberSpawnData.Info.UnitsPerGroupAmount,
            memberSpawnData.Info.SpawnTime, new StumpTarget(null, transform.position));
    }

    public void SpawnMembers(GroupMember memberPrefab, int memberCount, float spawnTime, StumpTarget target)
    {
        spawnPoint.Spawn(memberPrefab, _group, _groupFormatter, spawnTime, memberCount, target, HandleSpawned);
    }

    private void HandleSpawned(IEnumerable<GroupMember> members)
    {
        foreach (var member in members)
        {
            _group.AddMember(member);
        }
    }
}
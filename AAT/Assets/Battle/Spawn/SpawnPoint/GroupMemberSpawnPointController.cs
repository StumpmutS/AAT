using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class GroupMemberSpawnPointController : SpawnPointController<List<GroupMember>>
{
    public void Spawn(GroupMember memberPrefab, Group group, GroupFormatter groupFormatter, float spawnTime, int amount, StumpTarget target, Action<List<GroupMember>> membersCallback)
    {
        StartCoroutine(CoSpawnUnits(memberPrefab, group, groupFormatter, spawnTime, amount, target, membersCallback));
    }
    
    private IEnumerator CoSpawnUnits(GroupMember memberPrefab, Group group, GroupFormatter groupFormatter, float spawnTime, int amount, StumpTarget target, Action<List<GroupMember>> membersCallback)
    {
        if (!Runner.IsServer) yield break;

        InvokeOnBeginSpawn();
        yield return new WaitForSeconds(spawnTime);
        
        List<GroupMember> members = new();
        for (int i = 0; i < amount; i++)
        {
            var pos = groupFormatter.GetPosition(group.GroupMembers.Count + 1 + i, group.GroupMembers.Count + i, target.Point);
            var member = Runner.Spawn(memberPrefab, pos, Quaternion.identity, Object.InputAuthority, InitUnit).GetComponent<GroupMember>();
            members.Add(member);
        }
        membersCallback?.Invoke(members);
        
        InvokeOnFinishedSpawn(members);

        void InitUnit(NetworkRunner _, NetworkObject o)
        {
            o.GetComponent<GroupMember>().Init(team.GetTeamNumber(), group);
        }
    }
}
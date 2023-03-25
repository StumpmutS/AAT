using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Actions/Spawn/Member Spawn On Calling Points")]
public class MemberSpawnOnCallingPointsGameAction : MemberSpawnGameAction
{
    [SerializeField] private bool includeSpawnOnTarget;
    
    protected override void SpawnMembers(GameActionInfo info, GroupMemberSpawner spawner)
    {
        var transform = GetTransform(info.TransformChain);
        var target = new StumpTarget(transform.gameObject, transform.position);
        spawner.SpawnMembers(memberSpawnData.Info.MemberPrefab, memberSpawnData.Info.UnitsPerGroupAmount, memberSpawnData.Info.SpawnTime, target);

        if (includeSpawnOnTarget) base.SpawnMembers(info, spawner);
    }
}
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Actions/Spawn/Member Spawn")]
public class MemberSpawnGameAction : AbilityGameAction
{
    [SerializeField] protected bool useCurrentGroup;
    [SerializeField, ShowIf(nameof(useCurrentGroup), false)] private Group groupPrefab;
    [SerializeField] protected MemberSpawnData memberSpawnData;

    public override void PerformAction(GameActionInfo info)
    {
        if (!info.MainCaller.Runner.IsServer) return;
        
        GroupMemberSpawner spawner;
        if (!useCurrentGroup)
        {
            var group = SpawnGroup(info);
            if (group.TryGetComponent<GroupMemberSpawner>(out spawner))
            {
                SpawnMembers(info, spawner);
            }
        }
        else if (info.MainCaller.TryGetComponent<GroupMemberSpawner>(out spawner))
        {
            SpawnMembers(info, spawner);
        }
        else
        {
            Debug.LogError($"{info.MainCaller.gameObject.name} does not contain a group member spawner component");
        }
    }

    private Group SpawnGroup(GameActionInfo info)
    {
        return info.MainCaller.Runner.Spawn(groupPrefab, info.Target.Point, Quaternion.identity, info.MainCaller.InputAuthority, BeforeSpawned);

        void BeforeSpawned(NetworkRunner _, NetworkObject o)
        {
            if (o.TryGetComponent<TeamController>(out var newTeam) && info.MainCaller.TryGetComponent<TeamController>(out var fromTeam))
            {
                newTeam.SetTeamNumber(fromTeam.GetTeamNumber());
            }
        }
    }

    protected virtual void SpawnMembers(GameActionInfo info, GroupMemberSpawner spawner)
    {
        spawner.SpawnMembers(memberSpawnData.Info.MemberPrefab, memberSpawnData.Info.UnitsPerGroupAmount, memberSpawnData.Info.SpawnTime, info.Target);
    }
}
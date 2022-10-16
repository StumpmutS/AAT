using System.Collections;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(GroupFormatter), typeof(Group))]
public class GroupMemberSpawner : SimulationBehaviour
{
    [SerializeField] private GroupMember memberPrefab;
    [SerializeField] private UnitSpawnData unitSpawnData;
    [SerializeField] private UnitSpawnPointController spawnPoint;

    private Group _group;
    private GroupFormatter _groupFormatter;

    private void Awake()
    {
        _group = GetComponent<Group>();
        _groupFormatter = GetComponent<GroupFormatter>();
    }

    private void Start()
    {
        SpawnMembers();
    }

    private void SpawnMembers()
    {
        for (int i = 0; i < unitSpawnData.UnitsPerGroupAmount; i++)
        {
            SpawnMember(_groupFormatter.GetPosition(unitSpawnData.UnitsPerGroupAmount, i, spawnPoint.transform.position), unitSpawnData.SpawnTime);
        }
    }

    private void SpawnMember(Vector3 pos, float spawnTime)
    {
        StartCoroutine(CoSpawnMember(pos, spawnTime));
    }

    private IEnumerator CoSpawnMember(Vector3 pos, float spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);
        var member = Runner.Spawn(memberPrefab, pos, Quaternion.identity);
        _group.AddMember(member);
    }
}
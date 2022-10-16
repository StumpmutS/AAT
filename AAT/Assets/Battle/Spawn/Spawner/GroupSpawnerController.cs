using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class GroupSpawnerController : NetworkBehaviour
{
    [SerializeField] private List<GroupSpawnPointController> spawnPoints;
    [SerializeField] private Group groupPrefab;
    
    public UnitSpawnData SpawnData { get; private set; }
    public List<Group> Groups { get; private set; } = new();

    private StatsManager _stats;
    private SectorController _sector;
    private int _groupQueue;
    private HashSet<GroupSpawnPointController> _openSpawnPoints;

    public event Action<IEnumerable<UnitController>> OnUnitsSpawned = delegate { };

    private void Awake()
    {
        _stats = GetComponent<StatsManager>();
        _openSpawnPoints = new HashSet<GroupSpawnPointController>(spawnPoints);
    }

    public void Init(UnitSpawnData spawnData, SectorController sector)
    {
        SpawnData = spawnData;
        _sector = sector;
        _stats.Init(spawnData.baseUnitStatsData);
    }

    public override void Spawned()
    {
        SpawnerManager.Instance.AddSpawner(this);
    }

    private void Start()
    {
        if (!Runner.IsServer) return;
        
        BeginSpawning();
    }

    private void BeginSpawning()
    {
        _groupQueue = SpawnData.SpawnGroupsAmount;
        CheckQueue();
    }

    private void CheckQueue()
    {
        foreach (var point in new HashSet<GroupSpawnPointController>(spawnPoints)) //make copy so we can add/remove items
        {
            if (_groupQueue < 1) return;

            _groupQueue--;
            _openSpawnPoints.Remove(point);
            SpawnGroup(point);
        }
    }

    private void SpawnGroup(GroupSpawnPointController point)
    {
        point.OnCancelledSpawn += HandleSpawnStop;
        point.OnFinishedSpawn += HandleSpawnStop;
        point.SpawnGroup(groupPrefab, SpawnData.SpawnTime, _sector);
    }

    private void HandleSpawnStop(SpawnPointController point)
    {
        point.OnCancelledSpawn -= HandleSpawnStop;
        point.OnFinishedSpawn -= HandleSpawnStop;
        _openSpawnPoints.Add((GroupSpawnPointController) point);
        CheckQueue();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        SpawnerManager.Instance.RemoveSpawner(this);
    }
}

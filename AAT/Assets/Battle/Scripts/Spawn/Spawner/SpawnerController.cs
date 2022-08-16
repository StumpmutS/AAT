using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using Utility.Scripts;

public class SpawnerController : NetworkBehaviour
{
    [SerializeField] private List<SpawnPointController> spawnPoints;
    [SerializeField] private UnitGroupController groupPrefab;
    
    public UnitSpawnData SpawnData { get; private set; }
    public List<UnitGroupController> Groups { get; private set; } = new();

    private SectorController _sector;
    private UnitStatsModifierManager _stats;
    private Queue<UnitGroupController> _groupQueue = new();
    private Dictionary<UnitGroupController, List<Coroutine>> _groupSpawnCoroutines = new();
    private int _openPointCount;

    public event Action<IEnumerable<UnitController>> OnUnitsSpawned = delegate { };

    private void Awake()
    {
        _stats = GetComponent<UnitStatsModifierManager>();
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

        _openPointCount = SpawnData.MaxSpawnLocationUse;
        BeginSpawning();
    }

    private void BeginSpawning()
    {
        for (int i = 0; i < SpawnData.SpawnGroupsAmount; i++)
        {
            var group = Runner.Spawn(groupPrefab);
            Groups.Add(group);
            QueueUnitGroup(group);
        }
        
        CheckQueue();
    }

    public void QueueUnitGroup(UnitGroupController group)
    {
        if (!Runner.IsServer) return;

        if (_groupSpawnCoroutines.ContainsKey(group))
        {
            foreach (var co in _groupSpawnCoroutines[group]) 
            { 
                StopCoroutine(co); 
            }
            FinishGroupSpawn(group);
            SpawnUnits(group);
            return;
        }
        
        _groupQueue.EnqueueNoDuplicates(group);
    }

    private void SpawnUnits(UnitGroupController group)
    {
        _openPointCount--;
        var amount = SpawnData.UnitsPerGroupAmount - group.Units.Count;
        if (!_groupSpawnCoroutines.ContainsKey(group)) _groupSpawnCoroutines[group] = new List<Coroutine>();
        _groupSpawnCoroutines[group].Add(StartCoroutine(CoSpawnUnits(spawnPoints.First(sp => !sp.IsSpawning), group, amount)));
    }

    private void FinishGroupSpawn(UnitGroupController group)
    {
        _groupSpawnCoroutines.Remove(group);
        _openPointCount++;
    }

    private IEnumerator CoSpawnUnits(SpawnPointController spawnPoint, UnitGroupController group, int amount)
    {
        var co = StartCoroutine(spawnPoint.CoSpawnUnits(SpawnData.UnitPrefab, group, _sector, SpawnData.SpawnTime, amount, OnUnitsSpawned));
        _groupSpawnCoroutines[group].Add(co);
        yield return co;
        FinishGroupSpawn(group);
        
        CheckQueue();
    }

    private void CheckQueue()
    {
        while (_groupQueue.Count > 0 && _openPointCount > 0)
        {
            SpawnUnits(_groupQueue.Dequeue());
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        SpawnerManager.Instance.RemoveSpawner(this);
    }
}

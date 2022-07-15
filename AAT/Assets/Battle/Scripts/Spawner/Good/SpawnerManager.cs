using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private SpawnPlotManager spawnPlotManager;
    
    public static SpawnerManager Instance { get; private set; }

    private List<BaseSpawnerController> _spawners = new();
    private int _nextIndex;

    private static BaseSpawnerController _activeSpawner;

    private void Awake()
    {
        Instance = this;
        _spawners.Equalize(spawnPlotManager.SpawnerPlots.Count);
    }

    public void AddSpawnerPlot(BaseSpawnerController spawner)
    {
        _spawners[_nextIndex] = spawner;
        spawner.OnSpawnerSelect += SetActiveSpawner;
    }

    private void SetActiveSpawner(BaseSpawnerController spawnerToSet) => _activeSpawner = spawnerToSet;

    public void SetNextSpawnerPlotIndex(int index)
    {
        _nextIndex = index;
    }

    public void UpgradeActiveSpawnerStats()
    {
        _activeSpawner.ModifyUnitGroupStats();
    }
}

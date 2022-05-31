using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private SpawnPlotManager spawnPlotManager;
    
    public static SpawnerManager Instance { get; private set; }

    private List<BaseSpawnerController> spawners = new();
    private int _nextIndex;

    private static BaseSpawnerController _activeSpawner;

    private void Awake()
    {
        Instance = this;
        spawners.Equalize(spawnPlotManager.SpawnerPlots.Count);
    }

    private void Start()
    {
        InputManager.OnAlpha1 += SelectSpawnerByIndex;
        InputManager.OnAlpha2 += SelectSpawnerByIndex;
        InputManager.OnAlpha3 += SelectSpawnerByIndex;
        InputManager.OnAlpha4 += SelectSpawnerByIndex;
        InputManager.OnAlpha5 += SelectSpawnerByIndex;
        InputManager.OnAlpha6 += SelectSpawnerByIndex;
    }

    public void AddSpawnerPlot(BaseSpawnerController spawner)
    {
        spawners[_nextIndex] = spawner;
        spawner.OnSpawnerSelect += SetActiveSpawner;
    }

    private void SetActiveSpawner(BaseSpawnerController spawnerToSet) => _activeSpawner = spawnerToSet;

    public void SetNextSpawnerPlotIndex(int index)
    {
        _nextIndex = index;
    }

    private void SelectSpawnerByIndex(int keyPressed)
    {
        foreach (var spawner in spawners.Where(spawner => spawner != null))
        {
            spawner.CurrentSpawnerVisualsSelectable.CallDeselect();
        }

        if (spawners[keyPressed - 1] != null)
        {
            spawners[keyPressed - 1].CurrentSpawnerVisualsSelectable.CallSelect();

        }
    }

    public void UpgradeActiveSpawnerStats()
    {
        _activeSpawner.ModifyUnitGroupStats();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private SpawnPlotManager spawnPlotManager;
    
    public static SpawnerManager Instance { get; private set; }

    private List<BaseSpawnerController> spawners = new List<BaseSpawnerController>();
    private int _nextIndex;

    private static BaseSpawnerController _activeSpawner;

    private void Awake()
    {
        Instance = this;
        spawners.Equalize(spawnPlotManager.SpawnerPlots.Count);
    }

    private void Start()
    {
        InputManager.OnNumberKey1 += SelectSpawnerByIndex;
        InputManager.OnNumberKey2 += SelectSpawnerByIndex;
        InputManager.OnNumberKey3 += SelectSpawnerByIndex;
        InputManager.OnNumberKey4 += SelectSpawnerByIndex;
        InputManager.OnNumberKey5 += SelectSpawnerByIndex;
        InputManager.OnNumberKey6 += SelectSpawnerByIndex;
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

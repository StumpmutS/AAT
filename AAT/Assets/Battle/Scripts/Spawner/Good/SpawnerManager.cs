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

    private void Start()
    {
        /*BaseInputManager.OnAlpha1 += SelectSpawnerByIndex;
        BaseInputManager.OnAlpha2 += SelectSpawnerByIndex;
        BaseInputManager.OnAlpha3 += SelectSpawnerByIndex;
        BaseInputManager.OnAlpha4 += SelectSpawnerByIndex;
        BaseInputManager.OnAlpha5 += SelectSpawnerByIndex;
        BaseInputManager.OnAlpha6 += SelectSpawnerByIndex;*/
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

    private void SelectSpawnerByIndex(int keyPressed)
    {
        foreach (var spawner in _spawners.Where(spawner => spawner != null))
        {
            spawner.CurrentSpawnerVisualsSelectable.CallDeselect();
        }

        if (_spawners[keyPressed - 1] != null)
        {
            _spawners[keyPressed - 1].CurrentSpawnerVisualsSelectable.CallSelect();

        }
    }

    public void UpgradeActiveSpawnerStats()
    {
        _activeSpawner.ModifyUnitGroupStats();
    }
}

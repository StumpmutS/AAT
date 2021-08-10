using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlotManager : MonoBehaviour
{
    [SerializeField] private List<SpawnerPlotController> spawnerPlots;

    private SpawnerPlotController activeSpawnerPlot;

    private void Awake()
    {
        foreach (var spawnerPlot in spawnerPlots)
        {
            spawnerPlot.OnSpawnerSelect += SetActiveSpawnerPlot;
        }
    }

    private void SetActiveSpawnerPlot(SpawnerPlotController spawnerPlot)
    {
        activeSpawnerPlot = spawnerPlot;
    }

    public void SetupActiveSpawner(UnitSpawnData unitSpawnData)
    {
        activeSpawnerPlot.SetupSpawner(unitSpawnData);
    }
}

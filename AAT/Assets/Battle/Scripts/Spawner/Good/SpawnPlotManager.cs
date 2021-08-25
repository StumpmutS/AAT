using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlotManager : MonoBehaviour
{
    [SerializeField] protected List<SpawnerPlotController> spawnerPlots;

    public List<SpawnerPlotController> SpawnerPlots => spawnerPlots;

    private SpawnerPlotController activeSpawnerPlot;
    private List<int> inactiveSpawnerPlotIndexes = new List<int>();

    private void Start()
    {
        foreach (var spawnerPlot in spawnerPlots)
        {
            spawnerPlot.OnSpawnerPlotSelect += SetActiveSpawnerPlot;
        }

        InputManager.OnNumberKey1 += SelectSpawnerPlotByIndex;
        InputManager.OnNumberKey2 += SelectSpawnerPlotByIndex;
        InputManager.OnNumberKey3 += SelectSpawnerPlotByIndex;
        InputManager.OnNumberKey4 += SelectSpawnerPlotByIndex;
        InputManager.OnNumberKey5 += SelectSpawnerPlotByIndex;
        InputManager.OnNumberKey6 += SelectSpawnerPlotByIndex;
    }

    private void SetActiveSpawnerPlot(SpawnerPlotController spawnerPlot)
    {
        activeSpawnerPlot = spawnerPlot;
    }

    public void SetupActiveSpawner(UnitSpawnData unitSpawnData)
    {
        for (int i = 0; i < spawnerPlots.Count; i++)
        {
            if (spawnerPlots[i] == activeSpawnerPlot)
            {
                SpawnerManager.SetNextSpawnerPlotIndex(i);
                inactiveSpawnerPlotIndexes.Add(i);
                break;
            }
        }

        activeSpawnerPlot.SetupSpawner(unitSpawnData);
    }

    private void SelectSpawnerPlotByIndex(int keyPressed)
    {
        for (int i = 0; i < spawnerPlots.Count; i++)
        {
            if (i != keyPressed - 1)
            {
                spawnerPlots[i].Deselect();
            }
        }
        if (!inactiveSpawnerPlotIndexes.Contains(keyPressed - 1))
        {
            spawnerPlots[keyPressed - 1].Select();
        }
    }
}

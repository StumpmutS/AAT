using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlotManager : MonoBehaviour
{
    [SerializeField] protected List<SpawnerPlotController> spawnerPlots;
    [SerializeField] private SpawnerPlotButtonsController buttonsController;

    public List<SpawnerPlotController> SpawnerPlots => spawnerPlots;

    private SpawnerPlotController _activeSpawnerPlot;
    private List<int> _inactiveSpawnerPlotIndexes = new List<int>();

    private void Start()
    {
        foreach (var spawnerPlot in spawnerPlots)
        {
            spawnerPlot.OnSpawnerPlotSelect += SetActiveSpawnerPlot;
            spawnerPlot.OnSpawnerPlotDeselect += RemoveButtons;
        }

        InputManager.OnNumberKey1 += SelectSpawnerPlotByIndex;
        InputManager.OnNumberKey2 += SelectSpawnerPlotByIndex;
        InputManager.OnNumberKey3 += SelectSpawnerPlotByIndex;
        InputManager.OnNumberKey4 += SelectSpawnerPlotByIndex;
        InputManager.OnNumberKey5 += SelectSpawnerPlotByIndex;
        InputManager.OnNumberKey6 += SelectSpawnerPlotByIndex;
        InputManager.OnNumberKey7 += SelectSpawnerPlotByIndex;
    }

    public void AddPlot(SpawnerPlotController plot)
    {
        spawnerPlots.Add(plot);
    }

    private void SetActiveSpawnerPlot(SpawnerPlotController spawnerPlot, EFaction faction)
    {
        _activeSpawnerPlot = spawnerPlot;
        buttonsController.DisplayByFaction(faction);
    }

    private void RemoveButtons()
    {
        buttonsController.RemoveButtons();
    }

    public void SetupActiveSpawner(UnitSpawnData unitSpawnData)
    {
        var index = spawnerPlots.IndexOf(_activeSpawnerPlot);
        
        SpawnerManager.Instance.SetNextSpawnerPlotIndex(index);
        _inactiveSpawnerPlotIndexes.Add(index);

        DeselectAllPlots();
        RemoveButtons();
        _activeSpawnerPlot.SetupSpawner(unitSpawnData);
    }

    private void SelectSpawnerPlotByIndex(int keyPressed)
    {
        DeselectAllPlots();
        if (!_inactiveSpawnerPlotIndexes.Contains(keyPressed - 1))
        {
            spawnerPlots[keyPressed - 1].CallSelect();
        }
    }

    private void DeselectAllPlots() => spawnerPlots.ForEach(plot => plot.CallDeselect());
}

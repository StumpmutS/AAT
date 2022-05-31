using System.Collections.Generic;
using UnityEngine;

public class SpawnPlotManager : MonoBehaviour
{
    [SerializeField] protected List<SpawnerPlotController> spawnerPlots;
    [SerializeField] private SpawnerPlotButtonsController buttonsController;

    public List<SpawnerPlotController> SpawnerPlots => spawnerPlots;

    private SpawnerPlotController _activeSpawnerPlot;
    private List<int> _inactiveSpawnerPlotIndexes = new();

    private void Start()
    {
        foreach (var spawnerPlot in spawnerPlots)
        {
            spawnerPlot.OnSpawnerPlotSelect += SetActiveSpawnerPlot;
            spawnerPlot.OnSpawnerPlotDeselect += RemoveButtons;
        }

        InputManager.OnAlpha1 += SelectSpawnerPlotByIndex;
        InputManager.OnAlpha2 += SelectSpawnerPlotByIndex;
        InputManager.OnAlpha3 += SelectSpawnerPlotByIndex;
        InputManager.OnAlpha4 += SelectSpawnerPlotByIndex;
        InputManager.OnAlpha5 += SelectSpawnerPlotByIndex;
        InputManager.OnAlpha6 += SelectSpawnerPlotByIndex;
        InputManager.OnAlpha7 += SelectSpawnerPlotByIndex;
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

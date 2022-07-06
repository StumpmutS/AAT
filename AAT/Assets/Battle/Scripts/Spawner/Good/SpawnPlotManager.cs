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

        BaseInputManager.OnAlpha1 += SelectSpawnerPlotByIndex;
        BaseInputManager.OnAlpha2 += SelectSpawnerPlotByIndex;
        BaseInputManager.OnAlpha3 += SelectSpawnerPlotByIndex;
        BaseInputManager.OnAlpha4 += SelectSpawnerPlotByIndex;
        BaseInputManager.OnAlpha5 += SelectSpawnerPlotByIndex;
        BaseInputManager.OnAlpha6 += SelectSpawnerPlotByIndex;
        BaseInputManager.OnAlpha7 += SelectSpawnerPlotByIndex;
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

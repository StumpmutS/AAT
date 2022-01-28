using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerPlotController : OutlineSelectableController //TODO: wtf
{
    [SerializeField] private RectTransform UnitsUIContainer;
    [SerializeField] private RectTransform UpgradesUIContainer;
    [SerializeField] private SpawnerController spawnerPrefab;
    [SerializeField] private SectorController sector;

    public event Action<SpawnerPlotController> OnSpawnerPlotSelect = delegate { };

    private void Start()
    {
        HideUnitSpawnerButtons();
    }

    protected override void Select()
    {
        base.Select();
        DisplayUnitSpawnerButtons();
        OnSpawnerPlotSelect.Invoke(this);
        Cursor.lockState = CursorLockMode.None;
    }

    protected override void Deselect()
    {
        base.Deselect();
        HideUnitSpawnerButtons();
    }

    private void DisplayUnitSpawnerButtons()
    {
        UnitsUIContainer.gameObject.SetActive(true);
    }

    private void HideUnitSpawnerButtons()
    {
        UnitsUIContainer.gameObject.SetActive(false);
    }

    public void SetupSpawner(UnitSpawnData spawnData)
    {
        SpawnerController instantiatedSpawner = Instantiate(spawnerPrefab, transform.position, transform.rotation);
        SpawnerManager.AddSpawnerPlot(instantiatedSpawner);
        instantiatedSpawner.Setup(spawnData, sector, UpgradesUIContainer);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerPlotController : EntityController
{
    [SerializeField] private RectTransform UnitsUIContainer;
    [SerializeField] private SpawnerController spawnerPrefab;

    public event Action<SpawnerPlotController> OnSpawnerPlotSelect = delegate { };

    private void Start()
    {
        HideUnitSpawnerButtons();
    }

    public override void Select()
    {
        base.Select();
        DisplayUnitSpawnerButtons();
        OnSpawnerPlotSelect.Invoke(this);
        Cursor.lockState = CursorLockMode.None;
    }

    public override void Deselect()
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
        instantiatedSpawner.Setup(spawnData);
    }
}

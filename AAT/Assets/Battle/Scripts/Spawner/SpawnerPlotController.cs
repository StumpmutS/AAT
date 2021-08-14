using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EntityController))]
public class SpawnerPlotController : MonoBehaviour
{
    [SerializeField] private RectTransform UnitsUIContainer;
    [SerializeField] private GameObject spawnerPrefab;

    public event Action<SpawnerPlotController> OnSpawnerSelect = delegate { };
    
    private EntityController entity;

    private void Awake()
    {
        entity = GetComponent<EntityController>();
        entity.OnSelect += Select;
        entity.OnDeselect += Deselect;
    }

    private void Start()
    {
        HideUnitSpawners();
    }

    private void Select()
    {
        DisplayUnitSpawners();
        OnSpawnerSelect.Invoke(this);
        Cursor.lockState = CursorLockMode.None;
    }

    private void Deselect()
    {
        HideUnitSpawners();
    }

    private void DisplayUnitSpawners()
    {
        UnitsUIContainer.gameObject.SetActive(true);
    }

    private void HideUnitSpawners()
    {
        UnitsUIContainer.gameObject.SetActive(false);
    }

    public void SetupSpawner(UnitSpawnData spawnData)
    {
        GameObject instantiatedSpawner = Instantiate(spawnerPrefab, transform.position, transform.rotation);
        instantiatedSpawner.GetComponent<SpawnerController>().Setup(spawnData);
    }
}

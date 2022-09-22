using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointVisualsController : MonoBehaviour
{
    [SerializeField] private List<UnitVisualComponent> visuals;

    private SpawnPointController _spawnPoint;

    private void Awake()
    {
        _spawnPoint = GetComponent<SpawnPointController>();
    }

    private void Update()
    {
        if (_spawnPoint.IsSpawning)
        {
            DisplayVisuals();
        }
        else
        {
            RemoveVisuals();
        }
    }

    private void DisplayVisuals()
    {
        foreach (var visual in visuals)
        {
            visual.ActivateComponent(null, transform.position);
        }
    }

    private void RemoveVisuals()
    {
        foreach (var visual in visuals)
        {
            visual.DeactivateComponent(null);
        }
    }
}

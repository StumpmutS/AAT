using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnPointController))]
public class SpawnPointVisualsController : MonoBehaviour
{
    [SerializeField] private List<UnitVisualComponent> visuals;

    private SpawnPointController _spawnPoint;

    private void Awake()
    {
        _spawnPoint = GetComponent<SpawnPointController>();
        _spawnPoint.OnBeginSpawn += DisplayVisuals;
        _spawnPoint.OnCancelledSpawn += RemoveVisuals;
        _spawnPoint.OnFinishedSpawn += RemoveVisuals;
    }

    private void DisplayVisuals(SpawnPointController _)
    {
        foreach (var visual in visuals)
        {
            visual.ActivateComponent(null, transform.position);
        }
    }

    private void RemoveVisuals(SpawnPointController _)
    {
        foreach (var visual in visuals)
        {
            visual.DeactivateComponent(null);
        }
    }

    private void OnDestroy()
    {
        if (_spawnPoint != null) _spawnPoint.OnBeginSpawn -= DisplayVisuals;
        if (_spawnPoint != null) _spawnPoint.OnCancelledSpawn -= RemoveVisuals;
        if (_spawnPoint != null) _spawnPoint.OnFinishedSpawn -= RemoveVisuals;
    }
}

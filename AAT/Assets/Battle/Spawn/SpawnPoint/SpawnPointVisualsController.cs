using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnPointVisualsController<T> : MonoBehaviour, IGameActionInfoGetter
{
    [SerializeField] private List<VisualGameAction> visuals;

    private SpawnPointController<T> _spawnPoint;

    private void Awake()
    {
        _spawnPoint = GetComponent<SpawnPointController<T>>();
        _spawnPoint.OnBeginSpawn += DisplayVisuals;
        _spawnPoint.OnCancelledSpawn += HandleSpawnCancelled;
        _spawnPoint.OnFinishedSpawn += HandleSpawnFinished;
    }

    private void DisplayVisuals(SpawnPointController<T> _)
    {
        GameActionRunner.Instance.PerformActions(visuals, this);
    }

    private void HandleSpawnCancelled(SpawnPointController<T> _)
    {
        RemoveVisuals();
    }

    private void HandleSpawnFinished(SpawnPointController<T> _, T u)
    {
        RemoveVisuals();
    }

    private void RemoveVisuals()
    {
        GameActionRunner.Instance.StopActions(visuals, this);
    }

    public IEnumerable<GameActionInfo> GetInfo()
    {
        return new []
        {
            new GameActionInfo(_spawnPoint.Object, SectorFinder.FindSector(_spawnPoint.transform.position, .5f,
                LayerManager.Instance.GroundLayer), new TransformChain(new[] {_spawnPoint.transform}),
            new StumpTarget(gameObject, _spawnPoint.transform.position))
        };
    }

    private void OnDestroy()
    {
        if (_spawnPoint != null) _spawnPoint.OnBeginSpawn -= DisplayVisuals;
        if (_spawnPoint != null) _spawnPoint.OnCancelledSpawn -= HandleSpawnCancelled;
        if (_spawnPoint != null) _spawnPoint.OnFinishedSpawn -= HandleSpawnFinished;
    }
}
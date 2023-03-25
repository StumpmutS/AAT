using System;
using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

public class TraverseAgentComponentState : AgentComponentState, IAgent
{
    [SerializeField] private float traverseSpeedPercentMultiplier = 200;

    private SectorReference _sectorReference;
    private StatsManager _stats;
    private float _speed => _stats.GetModifiedStat(EUnitFloatStats.MovementSpeed) * traverseSpeedPercentMultiplier / 100;
    private float _speedMultiplier;

    private bool _entered;
    private bool _activationReady;
    private HashSet<object> _disablers = new();
    private bool _enabled;
    private bool _moving;
    private Vector3 _destination;
    private Vector3 _startPosition;
    private float _targetDistanceSqr;

    public event Action OnPathSet = delegate { };
    public event Action OnPathFinished = delegate { };
    public event Action OnWarped = delegate { };

    protected override void OnSpawnSuccess()
    {
        _stats = Container.GetComponent<StatsManager>();
        _sectorReference = Container.GetComponent<SectorReference>();
    }

    protected override void OnEnter()
    {
        _entered = true;
        _activationReady = false;
    }

    protected override void Tick()
    {
        if (!_enabled || !_moving) return;
        Traverse();
    }

    private void Traverse()
    {
        var newPos = Vector3.MoveTowards(Container.transform.position, _destination, _speed * Runner.DeltaTime);
        if ((newPos - _startPosition).sqrMagnitude >= _targetDistanceSqr) EndTraverse();
        Container.transform.LookAt(newPos);
        Container.transform.position = newPos;
    }
    
    private void EndTraverse()
    {
        OnPathFinished.Invoke();
        ClearDestination();
    }

    public override void OnExit()
    {
        _brain.GetBlackboard().TraverseAgentReady = false;
        _entered = false;
    }

    public void Activate() => _brain.GetBlackboard().TraverseAgentReady = true;

    public bool IsActive() => _entered;
    
    public bool CalculateTeleportPath(Vector3 destination, out List<TeleportPoint> points)
    {
        points = null;
        var targetSector = SectorFinder.FindSector(destination, 3, LayerManager.Instance.GroundLayer);

        var totalTime = SectorManager.Instance.PathBetween(Container.transform.position, _speed, _sectorReference.Sector, targetSector, out var teleportPoints);
        if (totalTime < 0 || totalTime > Vector3.Distance(Container.transform.position, destination) / _speed) return true;
        points = teleportPoints;
        return true;
    }

    public void SetDestination(Vector3 destination)
    {
        destination.y = Container.transform.position.y;
        _destination = destination;
        _startPosition = Container.transform.position;
        _targetDistanceSqr = (_destination - _startPosition).sqrMagnitude;
        _moving = true;
        OnPathSet.Invoke();
    }

    public void ClearDestination() => _moving = false;

    public void EnableAgent(object caller)
    {
        _disablers.Remove(caller);
        if (_disablers.Count <= 0) _enabled = true;
    }

    public void DisableAgent(object caller)
    {
        _disablers.Add(caller);
        _enabled = false;
    }

    public float GetSpeed() => _moving ? _speed : 0;

    public void Warp(Vector3 position)
    {
        Container.transform.position = new Vector3(position.x, Container.transform.position.y, position.z);
        OnWarped.Invoke();
    }
}

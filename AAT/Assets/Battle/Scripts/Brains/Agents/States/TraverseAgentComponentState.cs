using System;
using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

public class TraverseAgentComponentState : ComponentState, IAgent
{
    private UnitController _unit;
    private UnitStatsModifierManager _stats;
    private float _speed => _stats.GetStat(EUnitFloatStats.MovementSpeed) * _stats.GetStat(EUnitFloatStats.TraverseSpeedPercentMultiplier) / 100;
    private float _speedMultiplier;

    private bool _entered;
    private bool _activationReady;
    private HashSet<object> _disablers = new();
    private bool _enabled;
    private bool _moving;
    private Vector3 _destination;
    private Vector3 _startPosition;
    private float _targetDistanceSqr;

    public event Action OnPathFinished = delegate { };

    public override void Spawned()
    {
        base.Spawned();
        _unit = Container.GetComponent<UnitController>();
        _stats = _unit.Stats;
    }

    protected override void OnEnter()
    {
        _entered = true;
        _activationReady = false;
    }

    public override void Tick()
    {
        base.Tick();
        if (!_enabled || !_moving) return;
        Traverse();
        CheckSector(Container.transform.position);
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
        CheckSector(_destination);
        OnPathFinished.Invoke();
        ClearDestination();
    }

    private void CheckSector(Vector3 point)
    {
        var sector = SectorFinder.FindSector(point, 5, LayerManager.Instance.GroundLayer);
        if (sector != _unit.Sector)
        {
            _unit.SetSector(sector);
        }
    }

    public override void OnExit()
    {
        _activationReady = false;
        _entered = false;
    }

    public override bool Decision() => _activationReady;

    public void Activate() => _activationReady = true;

    public bool IsActive() => _entered;
    
    public bool CalculateTeleportPath(Vector3 destination, out List<TeleportPoint> points, out Vector3 finalDestination)
    {
        points = null;
        finalDestination = destination;
        var targetSector = SectorFinder.FindSector(destination, 3, LayerManager.Instance.GroundLayer);

        var totalTime = SectorManager.Instance.PathBetween(Container.transform.position, _speed, _unit.Sector, targetSector, out var teleportPoints);
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
    }

    public Vector3 GetDestination() => _destination;

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

    public void SetSpeedMultiplier(float speedMultiplier)
    {
        _speedMultiplier = speedMultiplier;
    }

    public float GetSpeed() => _speed * _speedMultiplier;

    public void Warp(Vector3 position) => Container.transform.position = new Vector3(position.x, Container.transform.position.y, position.z);
}

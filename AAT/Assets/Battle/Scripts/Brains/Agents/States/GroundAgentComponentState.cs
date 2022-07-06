using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utility.Scripts;

public class GroundAgentComponentState : ComponentState, IAgent
{
    private UnitController _unit;
    private UnitStatsModifierManager _stats;
    private NavMeshAgent _agent;

    private bool _activationReady;
    private bool _enabled;
    private HashSet<object> _disablers = new();
    private bool _pathSet;
    private Vector3 _destination;
    
    public event Action OnPathFinished = delegate { };
    
    private void Awake()
    {
        _unit = GetComponent<UnitController>();
        _stats = _unit.Stats;
        _agent = GetComponent<NavMeshAgent>();
    }
    
    public override void OnEnter()
    {
        _enabled = true;
        EnableAgent(this);
        _activationReady = false;
    }

    public override void Tick()
    {
        base.Tick();
        if (!_agent.enabled) return;
        if (_agent.pathPending) return;
        if (_agent.hasPath) _pathSet = true;
        if (!_pathSet) return;
        if (_agent.remainingDistance >= .1f) return;
        FinishPath();
    }

    public override void OnExit()
    {
        _enabled = false;
        _activationReady = false;
    }

    public override bool Decision() => _activationReady;

    public void Activate() => _activationReady = true;

    public bool IsActive() => _enabled;
    
    public bool CalculateTeleportPath(Vector3 destination, out List<TeleportPoint> points, out Vector3 finalDestination)
    {
        points = null;
        finalDestination = destination;
        var fromSector = _unit.Sector;
        var targetSector = SectorFinder.FindSector(destination, 3, LayerManager.Instance.GroundLayer);

        return SectorManager.Instance.PathBetween(transform.position, _agent.speed, fromSector, targetSector, out points) >= 0;
    }

    public void SetDestination(Vector3 destination)
    {
        var fromSector = _unit.Sector;
        var targetSector = SectorFinder.FindSector(destination, 3, LayerManager.Instance.GroundLayer);
        if (fromSector != targetSector) return;
        _pathSet = false;
        _agent.SetDestination(new Vector3(destination.x, transform.position.y, destination.z));
    }

    public Vector3 GetDestination() => _destination;

    private void FinishPath()
    {
        _agent.ResetPath();
        _pathSet = false;
        OnPathFinished.Invoke();
    }

    public void ClearDestination()
    {
        _agent.ResetPath();
    }

    public void SetSpeed(float speed)
    {
        _stats.ModifyFloatStat(EUnitFloatStats.MovementSpeed, _stats.GetStat(EUnitFloatStats.MovementSpeed) - speed);
        _agent.speed = _stats.GetStat(EUnitFloatStats.MovementSpeed);
    }

    public float GetSpeed() => _agent.speed;

    public void EnableAgent(object caller)
    {
        _disablers.Remove(caller);
        if (_disablers.Count <= 0) _agent.enabled = true;
    }

    public void DisableAgent(object caller)
    {
        _disablers.Add(caller);
        _agent.enabled = false;
    }

    public void Warp(Vector3 desiredPos)
    {
        if (_agent.enabled)
        {
            _agent.Warp(desiredPos);
            return;
        }

        transform.position = new Vector3(desiredPos.x, transform.position.y, desiredPos.z);
    }
}
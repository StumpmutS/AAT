using System;
using System.Collections.Generic;
using UnityEngine;


public class FlyingAgentComponentState : AgentComponentState, IAgent
{
    private FlyingController _flyingController;
    private StatsManager _stats;
    private float _speed => _stats.GetModifiedStat(EUnitFloatStats.MovementSpeed);
    private float _speedMultiplier = 1;
    
    private bool _entered;
    private bool _activationReady;
    private bool _enabled;
    private HashSet<object> _disablers = new();
    private float _origY;
    private bool _moving;
    private Vector3 _destination;

    public event Action OnPathSet = delegate { };
    public event Action OnPathFinished = delegate { };
    public event Action OnWarped = delegate { };

    protected override void OnSpawnSuccess()
    {
        _stats = Container.GetComponent<StatsManager>();
        _flyingController = Container.GetComponent<FlyingController>();
        _flyingController.OnArrival += FinishPath;
    }

    private void FinishPath() => OnPathFinished.Invoke();

    private void Start()
    {
        _origY = transform.position.y;
    }

    protected override void OnEnter()
    {
        _activationReady = false;
        _entered = true;
    }

    protected override void Tick()
    {
        if (!_enabled || !_moving) return;
        _flyingController.Move();
    }

    public override void OnExit() 
    {
        _activationReady = false;
        _entered = false;
    }

    public void Activate() => _activationReady = true;

    public bool IsActive() => _entered;

    public bool CalculateTeleportPath(Vector3 destination, out List<TeleportPoint> points)
    {
        points = null;
        return true;
    }

    public void SetDestination(Vector3 destination)
    {
        destination.y = _origY;
        if (_destination != destination)
        {
            _moving = true;
            OnPathSet.Invoke();
        }
        _destination = destination;
        _flyingController.SetDestination(destination);
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
        ClearDestination();
    }

    public float GetSpeed() => _moving ? _speed : 0;

    public void Warp(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y, position.z);
        OnWarped.Invoke();
    }
}

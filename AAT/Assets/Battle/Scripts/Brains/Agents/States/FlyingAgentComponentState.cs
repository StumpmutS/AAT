using System;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAgentComponentState : ComponentState, IAgent
{
    private FlyingController _flyingController;
    private UnitStatsModifierManager _stats;
    private float _speed => _stats.CurrentStats[EUnitFloatStats.MovementSpeed];
    
    private bool _entered;
    private bool _activationReady;
    private bool _enabled;
    private HashSet<object> _disablers = new();
    private float _origY;
    private bool _moving;
    private Vector3 _destination;
    
    public event Action OnPathFinished = delegate { };

    private void Awake()
    {
        _stats = GetComponent<UnitStatsModifierManager>();
        _flyingController = GetComponent<FlyingController>();
        _flyingController.OnArrival += FinishPath;
    }

    private void FinishPath() => OnPathFinished.Invoke();

    private void Start() => _origY = transform.position.y;

    public override void OnEnter()
    {
        _activationReady = false;
        _entered = true;
    }

    public override void Tick()
    {
        base.Tick();
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        if (Input.GetKeyDown(KeyCode.R)) SetDestination(transform.position);
        if (!_enabled || !_moving) return;
        _flyingController.Fly(_destination);
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
        return true;
    }

    public void SetDestination(Vector3 destination)
    {
        destination.y += _origY;
        if (_destination != destination) 
            _moving = true;
        _destination = destination;
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
        ClearDestination();
    }

    public void SetSpeed(float speed) => _stats.ModifyFloatStat(EUnitFloatStats.MovementSpeed, _stats.CurrentStats[EUnitFloatStats.MovementSpeed] - speed);

    public float GetSpeed() => _speed;

    public void Warp(Vector3 position) => transform.position = new Vector3(position.x, _origY + position.y, position.z);
}

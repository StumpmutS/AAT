using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectableController))]
public class AIPlayerOverrideController : AIPathfinder
{
    private SelectableController _selectable;
    private bool _movementOverride;
    private bool MovementOverride
    {
        get => _movementOverride;
        set
        {
            if (value == _movementOverride) return;
            _movementOverride = value;
            if (!value) InputManager.OnUpdate -= CheckTargetDistance;
            else InputManager.OnUpdate += CheckTargetDistance;
        }
    }

    public event Action OnReroute = delegate { };

    protected override void Awake()
    {
        base.Awake();
        _selectable = GetComponent<SelectableController>();
        _selectable.OnSelect += SubscribeToInputManager;
        _selectable.OnDeselect += UnsubscribeFromInputManager;
    }

    private void SubscribeToInputManager()
    {
        InputManager.OnRightClickDown += SetTargetDestination;
    }

    private void UnsubscribeFromInputManager()
    {
        InputManager.OnRightClickDown -= SetTargetDestination;
    }

    public void SetTargetDestination()
    {
        if (!_active) return;
        Reroute();
        MovementOverride = true;
        if (unitAnimationController != null)
            unitAnimationController.SetMovement(_movementSpeed);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit)) return;
        var destination = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        _agent.SetDestination(destination);
    }

    protected override void Attack(Collider target)
    {
        MovementOverride = false;
        base.Attack(target);
    }

    protected override void Chase(Collider target)
    {
        MovementOverride = false;
        base.Chase(target);
    }

    protected override void Patrol()
    {
        MovementOverride = false;
        base.Patrol();
    }

    protected override void NoAIState()
    {
        if (!MovementOverride)
            base.NoAIState();
    }

    private void CheckTargetDistance()
    {
        if (!_agent.AgentEnabled) return;
        if (_agent.RemainingDistance < .1f)
        {
            MovementOverride = false;
        }
    }

    private void Reroute()
    {
        OnReroute.Invoke();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        MovementOverride = false;
    }
}

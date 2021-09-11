using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class AIPlayerOverrideController : AIPathfinder
{
    private EntityController entity;

    private bool movementOverride;

    private bool MovementOverride
    {
        get { return movementOverride; }
        set
        {
            if (value == movementOverride) return;
            movementOverride = value;
            if (!value) InputManager.OnUpdate -= CheckTargetDistance;
            else InputManager.OnUpdate += CheckTargetDistance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        entity = GetComponent<EntityController>();
        entity.OnSelect += SubscribeToInputManager;
        entity.OnDeselect += UnsubscribeFromInputManager;
    }

    private void SubscribeToInputManager()
    {
        InputManager.OnRightClick += SetTargetDestination;
    }

    private void UnsubscribeFromInputManager()
    {
        InputManager.OnRightClick -= SetTargetDestination;
    }

    private void SetTargetDestination()
    {
        MovementOverride = true;
        if (unitAnimationController != null)
            unitAnimationController.SetMovement(movementSpeed);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            Vector3 destination = hit.point;
            agent.SetDestination(destination);
        }
    }

    protected override void Attack(GameObject target)
    {
        MovementOverride = false;
        base.Attack(target);
    }

    protected override void Chase(GameObject target)
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
        if (agent.remainingDistance < .1f)
        {
            MovementOverride = false;
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        MovementOverride = false;
    }
}

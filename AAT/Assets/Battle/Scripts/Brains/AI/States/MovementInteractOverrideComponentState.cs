using System;
using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

public class MovementInteractOverrideComponentState : MovementOverrideComponentState
{
    private AIBrain _aiBrain;
    private UnitController _unit;
    
    private Queue<Vector3> _queuedPoints = new();
    private Queue<InteractableController> _queuedInteractables = new();
    public InteractableController CurrentInteractable { get; private set; }
    private InteractionComponentState _currentInteractionComponentState;
    private bool _entered;
    private bool _base;

    protected override void Awake()
    {
        base.Awake();
        _aiBrain = GetComponent<AIBrain>();
        _unit = GetComponent<UnitController>();
    }

    public override void OnEnter()
    {
        _base = false;
        _entered = true;
        CurrentInteractable = null;
        _agent.EnableAgent(this);
        ClearQueue();
        base.OnEnter();
    }

    protected override void SetDestination()
    {
        var fromSector = _unit.Sector;
        var targetSector = SectorFinder.FindSector(BaseInputManager.RightClickUpPosition, 3, LayerManager.Instance.GroundLayer);
        
        SetDesiredInteractable(InteractableManager.Instance.GetHoveredInteractable(_unit));
        
        if (fromSector == targetSector)
        {
            if (CurrentInteractable != null)
            {
                SetDesiredInteractable(CurrentInteractable);
                _agent.SetDestination(CurrentInteractable.transform.position);
                return;
            }

            _base = true;
            base.SetDestination();
        }

        if (!_agent.CalculateTeleportPath(BaseInputManager.RightClickUpPosition, out var points, out var finalDestination)) return;
        if (points == null || points.Count < 1)
        {
            _base = true;
            base.SetDestination();
            return;
        }

        SetDesiredInteractable(points[0]);
        for (int i = 1; i < points.Count; i++)
        {
            QueuePoint(Vector3.zero, points[i]);
        }
        QueuePoint(finalDestination, CurrentInteractable);
    }

    public void SetDesiredInteractable(InteractableController interactable)
    {
        if (!IsInterruptable && _entered && CurrentInteractable != null) return;
        CurrentInteractable = interactable;
    }

    public override void Tick()
    {
        if (_base)
        {
            base.Tick();
            return;
        }

        if (_currentInteractionComponentState != null)
        {
            _currentInteractionComponentState.Tick();
            return;
        }
        
        if (CurrentInteractable == null)
        {
            CheckQueue();
            return;
        }
        
        if (Vector3.Distance(CurrentInteractable.transform.position, transform.position) <= CurrentInteractable.InteractRange)
        {
            ReachInteractable();
            return;
        }
        
        _agent.SetDestination(CurrentInteractable.transform.position);
    }

    private void ReachInteractable()
    {
        _currentInteractionComponentState = (InteractionComponentState) _aiBrain.AddState(CurrentInteractable.RequestState());
        CurrentInteractable.RequestAffection(_currentInteractionComponentState);
        _currentInteractionComponentState.OnInteractionFinished += HandleInteractionComponentFinished;
        _currentInteractionComponentState.OnEnter();
    }

    private void HandleInteractionComponentFinished()
    {
        _currentInteractionComponentState.OnInteractionFinished -= HandleInteractionComponentFinished;
        _currentInteractionComponentState.OnExit();
        _currentInteractionComponentState = null;
        CurrentInteractable = null;
        CheckQueue();
    }

    public override void OnExit()
    {
        base.OnExit();
        if (_currentInteractionComponentState != null)
        {
            _currentInteractionComponentState.OnInteractionFinished -= HandleInteractionComponentFinished;
            _currentInteractionComponentState = null;
        }
        CurrentInteractable = null;
        _entered = false;
    }
    
    private void QueuePoint(Vector3 point, InteractableController teleportPoint)
    {
        _queuedPoints.Enqueue(point);
        _queuedInteractables.Enqueue(teleportPoint);
    }
    
    private void CheckQueue()
    { 
        if (_queuedInteractables.Count > 0 && _queuedInteractables.Peek() != null)
        {
            SetDesiredInteractable(_queuedInteractables.Dequeue());
            _queuedPoints.Dequeue();
            return;
        }

        if (_queuedPoints.Count < 1)
        {
            _componentStateMachine.Exit(this);
            return;
        }
        _agent.SetDestination(_queuedPoints.Dequeue());
        _queuedInteractables.Dequeue();
    }
    
    private void ClearQueue()
    {
        _queuedPoints.Clear();
        _queuedInteractables.Clear();
    }
}

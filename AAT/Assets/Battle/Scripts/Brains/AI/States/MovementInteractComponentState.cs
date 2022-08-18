using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Utility.Scripts;

public class MovementInteractComponentState : ComponentState
{
    [Networked(OnChanged = nameof(OnCurrentSpeedChange))] private float CurrentSpeed { get; set; }
    public static void OnCurrentSpeedChange(Changed<MovementInteractComponentState> changed)
    {
        changed.Behaviour._animation.SetMovement(changed.Behaviour.CurrentSpeed);
    }
    
    private AgentBrain _agentBrain;
    protected IAgent _agent => _agentBrain.CurrentAgent;
    protected UnitController _unit;
    private UnitAnimationController _animation;
    private InteractingController _interactingController;
    
    private Queue<Tuple<Vector3, InteractableController>> _queuedTargets = new();
    private Tuple<Vector3, InteractableController> _currentTarget;
    private bool _interacting;

    public override void Spawned()
    {
        base.Spawned();
        _animation = Container.GetComponent<UnitAnimationController>();
        
        if (!Runner.IsServer) return;

        _agentBrain = Container.GetComponent<AgentBrain>();
        _unit = Container.GetComponent<UnitController>();
        _interactingController = Container.GetComponent<InteractingController>();
    }

    protected override void OnEnter()
    {
        _interacting = false;
        _currentTarget = null;
        _agent.EnableAgent(this);
        ClearQueue();
        SetDestination();
    }

    private void SetDestination()
    {
        _currentTarget = null;
        var hit = _unit.Runner.GetPlayerObject(_unit.Object.InputAuthority).GetComponent<Player>().NetworkInputHit;
        hit.GameObject.TryGetComponent<InteractableController>(out var interactable);
        /*if (TargetHelper.Relation(_unit, hit.gameobject.GetComponent<UnitController>(), ETargetRelation.Enemy)) {
            setup follow enemy
        }*/
        
        var clickPosition = hit.Point;
        clickPosition.y = 0;
        var fromSector = _unit.Sector;
        var targetSector = SectorFinder.FindSector(clickPosition, 3, LayerManager.Instance.GroundLayer);
        
        if (fromSector == targetSector)
        {
            if (interactable != null)
            {
                QueuePoint(interactable.transform.position, interactable);
                CheckQueue();
                return;
            }

            QueuePoint(clickPosition, null);
            CheckQueue();
            return;
        }

        if (!_agent.CalculateTeleportPath(clickPosition, out var points, out var finalDestination)) return;
        if (points == null || points.Count < 1)
        {
            QueuePoint(clickPosition, null);
            CheckQueue();
            return;
        }

        for (int i = 0; i < points.Count; i++)
        {
            QueuePoint(points[i].transform.position, points[i]);
        }
        
        if (interactable != null)
        {
            QueuePoint(interactable.transform.position, interactable);
            CheckQueue();
            return;
        }
        QueuePoint(finalDestination, null);
        CheckQueue();
    }

    public override void Tick()
    {
        if (_interacting) return;
        CurrentSpeed = _agent.GetSpeed();
        
        if (_currentTarget == null)
        {
            CheckQueue();
            return;
        }
        
        if (_currentTarget.Item2 != null)
        {
            if (Vector3.Distance(_currentTarget.Item2.transform.position, Container.transform.position) <= _currentTarget.Item2.InteractRange)
            {
                ReachInteractable();
                return;
            }
            
            _agent.SetDestination(_currentTarget.Item2.transform.position);
            return;
        }
        
        _agent.SetDestination(_currentTarget.Item1);
    }

    private void ReachInteractable()
    {
        _interacting = true;
        _interactingController.InteractWith(_currentTarget.Item2);
        _interactingController.OnInteractionFinished += HandleInteractionComponentFinished;
    }

    private void HandleInteractionComponentFinished()
    {
        _agent.OnPathFinished -= HandleAgentPathFinished;
        _interacting = false;
        _interactingController.OnInteractionFinished -= HandleInteractionComponentFinished;
        _currentTarget = null;
        CheckQueue();
    }

    private void HandleAgentPathFinished()
    {
        _agent.OnPathFinished -= HandleAgentPathFinished;
        CheckQueue();
    }

    public override void OnExit()
    {
        CurrentSpeed = 0;
        _agent.OnPathFinished -= HandleAgentPathFinished;
        _interactingController.OnInteractionFinished -= HandleInteractionComponentFinished;
        _currentTarget = null;
    }
    
    private void CheckQueue()
    {
        if (_queuedTargets.Count < 1)
        {
            _componentStateMachine.Exit(this);
            return;
        }

        _currentTarget = _queuedTargets.Dequeue();
        _agent.SetDestination(_currentTarget.Item1);
        _agent.OnPathFinished += HandleAgentPathFinished;
    }
    
    private void QueuePoint(Vector3 point, InteractableController teleportPoint)
    {
        _queuedTargets.Enqueue(new Tuple<Vector3, InteractableController>(point, teleportPoint));
    }
    
    private void ClearQueue() => _queuedTargets.Clear();
}

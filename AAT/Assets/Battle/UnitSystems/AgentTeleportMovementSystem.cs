using System;
using System.Collections.Generic;
using UnityEngine;

public class AgentTeleportMovementSystem : AgentMovementSystem
{
    [SerializeField] private InteractionBrain interactionBrain;

    private Queue<StumpTarget> _queuedTargets = new();
    
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (CurrentTarget == null || !CurrentTarget.Hit.TryGetComponent<InteractableController>(out var interactable) || !InteractionHelpers.InteractionReady(transform, interactable)) return;
        agentBrain.ClearDestination();
        InteractionHelpers.InitiateInteraction(interactionBrain, interactable, CheckQueue);
    }

    public override void SetTarget(StumpTarget target)
    {
        if (target == null || target.Hit == null) return;
        
        _queuedTargets.Clear();
        if (agentBrain.CalculateTeleportPath(target.Point, out var points))
        {
            foreach (var teleportPoint in points)
            {
                _queuedTargets.Enqueue(new StumpTarget(teleportPoint.gameObject, teleportPoint.transform.position));
            }
        }
        
        _queuedTargets.Enqueue(target);
        
        agentBrain.EnableAgent(this);

        CheckQueue();
    }

    private void CheckQueue()
    {
        if (_queuedTargets.Count < 1) return;
        
        CurrentTarget = _queuedTargets.Dequeue();
        agentBrain.SetDestination(CurrentTarget.Point);
        if (_queuedTargets.Count < 1) //Final destination (not tp)
        {
            agentBrain.OnPathFinished += HandlePathFinished;
        }
    }

    public override void Stop()
    {
        _queuedTargets.Clear();
        base.Stop();
    }

    public override void Disable()
    {
        _queuedTargets.Clear();
        base.Disable();
    }
}
using System;
using Fusion;
using UnityEngine;

public class AgentMovementSystem : SimulationBehaviour, IMoveSystem
{
    [SerializeField] protected AgentBrain agentBrain;
    [SerializeField] private Follower follower;

    private StumpTarget _currentTarget;
    protected StumpTarget CurrentTarget
    {
        get => _currentTarget;
        set
        {
            _following = follower.CanFollow(value);
            _currentTarget = value;
        }
    }
    private bool _following;

    public event Action OnPathFinished = delegate { };

    public override void FixedUpdateNetwork()
    {
        if (_following)
        {
            agentBrain.SetDestination(CurrentTarget.Hit.transform.position);
        }
    }

    public virtual void SetTarget(StumpTarget target)
    {
        if (target == null || target.Hit == null) return;

        CurrentTarget = target;
        agentBrain.EnableAgent(this);
        agentBrain.OnPathFinished += HandlePathFinished;
    }

    protected void HandlePathFinished()
    {
        agentBrain.OnPathFinished -= HandlePathFinished;
        FinishPath();
    }

    public void Warp(StumpTarget target)
    {
        agentBrain.Warp(target.Point);
    }

    public virtual void Stop()
    {
        agentBrain.ClearDestination();
    }

    public void Enable()
    {
        agentBrain.EnableAgent(this);
    }

    public virtual void Disable()
    {
        agentBrain.DisableAgent(this);
    }

    protected void FinishPath()
    {
        OnPathFinished.Invoke();
    }
}
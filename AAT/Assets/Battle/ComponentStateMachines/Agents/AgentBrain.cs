using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgentBrain : Brain<AgentTransitionBlackboard>, IAgent
{
    [SerializeField] private AgentTransition defaultTransition;
    [SerializeField] private List<AgentTransition> transitions;

    protected override Transition<AgentTransitionBlackboard> GetDefaultTransition() => defaultTransition;

    protected override List<Transition<AgentTransitionBlackboard>> GetTransitions() => transitions.Select(t => (Transition<AgentTransitionBlackboard>) t).ToList();

    protected override AgentTransitionBlackboard InitBlackboard() => new AgentTransitionBlackboard();
    
    private IAgent CurrentAgent => (IAgent) ComponentStateMachine.CurrentComponentState;
    private IAgent _cachedAgent;
    
    public event Action OnPathSet = delegate { };
    public event Action OnPathFinished = delegate { };
    public event Action OnWarped = delegate { };

    public override void Spawned()
    {
        base.Spawned();
        ComponentStateMachine.OnStateChanged += HandleStateChanged;
    }

    private void HandleStateChanged()
    {
        if (_cachedAgent != null)
        {
            _cachedAgent.OnPathSet -= InvokeOnPathSet;
            _cachedAgent.OnPathFinished -= InvokeOnPathFinished;
            _cachedAgent.OnWarped -= InvokeOnWarped;
        }
        CurrentAgent.OnPathSet += InvokeOnPathSet;
        CurrentAgent.OnPathFinished += InvokeOnPathFinished;
        CurrentAgent.OnWarped += InvokeOnWarped;
        _cachedAgent = CurrentAgent;
    }

    private void InvokeOnPathSet() => OnPathSet.Invoke();
    private void InvokeOnPathFinished() => OnPathFinished.Invoke();
    private void InvokeOnWarped() => OnWarped.Invoke();

    public void Activate()
    {
        CurrentAgent.Activate();
    }

    public bool IsActive()
    {
        return CurrentAgent.IsActive();
    }

    public bool CalculateTeleportPath(Vector3 destination, out List<TeleportPoint> points)
    {
        return CurrentAgent.CalculateTeleportPath(destination, out points);
    }

    public void SetDestination(Vector3 destination)
    {
        CurrentAgent.SetDestination(destination);
    }

    public void ClearDestination()
    {
        CurrentAgent.ClearDestination();
    }

    public void EnableAgent(object caller)
    {
        CurrentAgent.EnableAgent(caller);
    }

    public void DisableAgent(object caller)
    {
        CurrentAgent.DisableAgent(caller);
    }

    public float GetSpeed()
    {
        return CurrentAgent.GetSpeed();
    }

    public void Warp(Vector3 position)
    {
        CurrentAgent.Warp(position);
    }
}
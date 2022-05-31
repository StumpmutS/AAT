using System.Collections;
using UnityEngine;

public class IdleComponentState : ComponentState
{
    private AgentBrain _agentBrain;
    private IAgent _agent => _agentBrain.CurrentAgent;
    private UnitAnimationController _animation;

    private void Awake()
    {
        _agentBrain = GetComponent<AgentBrain>();
        _animation = GetComponent<UnitAnimationController>();
    }

    public override void OnEnter()
    {
        _agent.EnableAgent(this);
        _animation?.SetMovement(0);
        _agent.ClearDestination();
    }

    public override void OnExit() { }
}

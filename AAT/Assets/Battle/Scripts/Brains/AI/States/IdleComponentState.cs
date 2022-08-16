using System.Collections;
using UnityEngine;

public class IdleComponentState : ComponentState
{
    private AgentBrain _agentBrain;
    private IAgent _agent => _agentBrain.CurrentAgent;
    private UnitAnimationController _animation;

    public override void Spawned()
    {
        if (!Runner.IsServer) return;

        _agentBrain = Container.GetComponent<AgentBrain>();
        _animation = Container.GetComponent<UnitAnimationController>();
    }

    protected override void OnEnter()
    {
        if (!Runner.IsServer) return;
        
        _agent.EnableAgent(this);
        _animation?.SetMovement(0);
        _agent.ClearDestination();
    }

    public override void OnExit() { }
}

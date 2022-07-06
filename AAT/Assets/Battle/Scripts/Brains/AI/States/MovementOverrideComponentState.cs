using UnityEngine;

public class MovementOverrideComponentState : ComponentState
{
    private AgentBrain _agentBrain;
    protected IAgent _agent => _agentBrain.CurrentAgent;
    private UnitAnimationController _animation;

    protected virtual void Awake()
    {
        _agentBrain = GetComponent<AgentBrain>();
        _animation = GetComponent<UnitAnimationController>();
    }

    public override void OnEnter()
    {
        SetDestination();
        _animation.SetMovement(_agent.GetSpeed());
    }

    protected virtual void SetDestination()
    {
        _agent.SetDestination(BaseInputManager.RightClickUpPosition);
        _agent.OnPathFinished += Finish;
    }

    private void Finish()
    {
        _componentStateMachine.Exit(this);
    }

    public override void OnExit()
    {
        _agent.OnPathFinished -= Finish;
    }
}

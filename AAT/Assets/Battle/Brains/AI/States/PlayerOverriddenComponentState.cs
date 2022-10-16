using System;

public class PlayerOverriddenComponentState : ComponentState<AiTransitionBlackboard>
{
    private AgentBrain _agentBrain;
    protected IAgent _agent => _agentBrain.CurrentAgent;
    protected override void OnSpawnSuccess()
    {
        _agentBrain = Container.GetComponent<AgentBrain>();
    }

    protected override void OnEnter()
    {
        _agent.EnableAgent(this);
        var target = Player.RightClickTarget;
        DecideTargetAction(target);
    }

    protected override void Tick() { }

    private void DecideTargetAction(StumpTarget target)
    {
        throw new NotImplementedException();
        //todo check what it is, then activate relevant systems
        //also look to queue certain actions
        //possible candidate for state machine
    }

    private void Interact(StumpTarget target)
    {
        throw new NotImplementedException();
    }

    private void Attack(StumpTarget target)
    {
        throw new NotImplementedException();
    }

    private void Move(StumpTarget target)
    {
        GetComponent<IMoveSystem>().Move(target);
        throw new NotImplementedException();
    }

    public override void OnExit()
    {
        throw new NotImplementedException();
    }
}

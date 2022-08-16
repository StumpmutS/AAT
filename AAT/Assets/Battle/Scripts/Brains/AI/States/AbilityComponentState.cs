using UnityEngine;

public class AbilityComponentState : ComponentState
{
    private AbilityHandler _abilityHandler;
    private AgentBrain _agentBrain;
    private IAgent _agent => _agentBrain.CurrentAgent;
    private UnitAnimationController _unitAnimation;
    
    private bool _abilityUsed;

    public override void Spawned()
    {
        if (!Runner.IsServer) return;

        _abilityHandler = Container.GetComponent<AbilityHandler>();
        _abilityHandler.OnAbilityUsed += AbilityUsed;
        _agentBrain = Container.GetComponent<AgentBrain>();
        _unitAnimation = Container.GetComponent<UnitAnimationController>();
    }

    private void AbilityUsed(bool used)
    {
        if (used) _abilityUsed = true;
    }

    public override bool Decision() => _abilityUsed;

    protected override void OnEnter()
    {
        _abilityUsed = false;
        _agent.ClearDestination();
        _agent.DisableAgent(this);
        _unitAnimation.SetMovement(0);
    }

    public override void Tick()
    {
        base.Tick();
        if (_abilityHandler.ActiveAbilities.Count < 1) _componentStateMachine.Exit(this);
    }

    public override void OnExit()
    {
        _agent.EnableAgent(this);
    }
}

using UnityEngine;

public class AbilityComponentState : ComponentState<AiTransitionBlackboard>
{
    private AbilityHandler _abilityHandler;
    private AgentBrain _agentBrain;
    private IAgent _agent => _agentBrain.CurrentAgent;
    private UnitAnimationController _unitAnimation;
    
    private bool _abilityUsed;

    protected override void OnSpawnSuccess()
    {
        _unitAnimation = Container.GetComponent<UnitAnimationController>();
        _abilityHandler = Container.GetComponent<AbilityHandler>();
        _abilityHandler.OnAbilityUsed += AbilityUsed;
        _agentBrain = Container.GetComponent<AgentBrain>();
    }

    private void AbilityUsed(bool used)
    {
        if (used) _abilityUsed = true;
    }

    protected override void OnEnter()
    {
        _abilityUsed = false;
        _agent.ClearDestination();
        _agent.DisableAgent(this);
        _unitAnimation.SetMovement(0);
    }

    protected override void Tick()
    {
        if (_abilityHandler.ActiveAbilities.Count < 1) _stateMachine.Exit(this);
    }

    public override void OnExit()
    {
        _agent.EnableAgent(this);
    }
}

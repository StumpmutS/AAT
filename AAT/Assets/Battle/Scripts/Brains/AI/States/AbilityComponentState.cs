using UnityEngine;

public class AbilityComponentState : ComponentState
{
    private AbilityHandler _abilityHandler;
    private AgentBrain _agentBrain;
    private IAgent _agent => _agentBrain.CurrentAgent;
    private UnitAnimationController _unitAnimation;
    
    private bool _abilityUsed;

    private void Awake()
    {
        _abilityHandler = GetComponent<AbilityHandler>();
        _abilityHandler.OnAbilityUsed += AbilityUsed;
        _agentBrain = GetComponent<AgentBrain>();
        _unitAnimation = GetComponent<UnitAnimationController>();
    }

    private void AbilityUsed(bool used) => _abilityUsed = used;

    public override bool Decision() => _abilityUsed;

    public override void OnEnter()
    {
        _agent.ClearDestination();
        _agent.DisableAgent(this);
        _unitAnimation.SetMovement(0);
    }

    public override void Tick()
    {
        base.Tick();
        if (!_abilityUsed) _componentStateMachine.Exit(this);
    }

    public override void OnExit()
    {
        _abilityUsed = false;
        _agent.EnableAgent(this);
    }
}

using Fusion;

public class ChaseComponentState : ComponentState
{
    [Networked(OnChanged = nameof(OnCurrentSpeedChange))] public NetworkBool Chasing { get; set; }
    public static void OnCurrentSpeedChange(Changed<ChaseComponentState> changed)
    {
        if (!changed.Behaviour.Runner.IsServer) return;
        changed.Behaviour._animation.SetChase(changed.Behaviour.Chasing);
    }

    private TargetFinder _targetFinder;
    private AgentBrain _agentBrain;
    private IAgent _agent => _agentBrain.CurrentAgent;
    private UnitAnimationController _animation;
    private UnitStatsModifierManager _unitStats;
    private float _chaseSpeedPercentMultiplier => _unitStats.GetStat(EUnitFloatStats.ChaseSpeedPercentMultiplier);

    public override void Spawned()
    {
        if (!Runner.IsServer) return;

        _targetFinder = Container.GetComponent<TargetFinder>();
        _agentBrain = Container.GetComponent<AgentBrain>();
        _unitStats = Container.GetComponent<UnitStatsModifierManager>();
        _animation = Container.GetComponent<UnitAnimationController>();
    }

    protected override void OnEnter()
    {
        if (!Runner.IsServer) return;

        _agent.EnableAgent(this);
        if (_targetFinder.SightTarget != null) _agent.SetDestination(_targetFinder.SightTarget.transform.position);
        _agent.SetSpeedMultiplier(_chaseSpeedPercentMultiplier / 100);
        Chasing = true;
    }

    public override void Tick()
    {
        base.Tick();
        if (_targetFinder.SightTarget != null)
        {
            _agent.SetDestination(_targetFinder.SightTarget.transform.position);
        }
        else
        {
            _componentStateMachine.Exit(this);
        }
    }

    public override void OnExit()
    {
        _agent.SetSpeedMultiplier(1);
        Chasing = false;
    }
}

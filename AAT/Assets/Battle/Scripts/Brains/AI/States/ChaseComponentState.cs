public class ChaseComponentState : ComponentState
{
    private TargetFinder _targetFinder;
    private AgentBrain _agentBrain;
    private IAgent _agent => _agentBrain.CurrentAgent;
    private UnitStatsModifierManager _unitStats;
    private UnitAnimationController _animation;
    private float _moveSpeed => _unitStats.GetStat(EUnitFloatStats.MovementSpeed);
    private float _chaseSpeedPercentMultiplier => _unitStats.GetStat(EUnitFloatStats.ChaseSpeedPercentMultiplier);

    private void Awake()
    {
        _targetFinder = GetComponent<TargetFinder>();
        _agentBrain = GetComponent<AgentBrain>();
        _unitStats = GetComponent<UnitStatsModifierManager>();
        _animation = GetComponent<UnitAnimationController>();
    }

    public override void OnEnter()
    {
        _agent.EnableAgent(this);
        _agent.SetDestination(_targetFinder.Target.transform.position);
        _agent.SetSpeed(_moveSpeed * _chaseSpeedPercentMultiplier / 100);
        _animation?.SetChase(true);
    }

    public override void Tick()
    {
        base.Tick();
        if (_targetFinder.Target is not null)
        {
            _agent.SetDestination(_targetFinder.Target.transform.position);
        }
    }

    public override void OnExit()
    {
        _agent.SetSpeed(_moveSpeed);
        _animation?.SetChase(false);
    }
}

using Fusion;
using UnityEngine;

public class ChaseComponentState : AiComponentState
{
    [SerializeField] private StatModifier statMod;

    public TargetFinder TargetFinder { get; private set; }
    private IMoveSystem _moveSystem;
    private EffectContainer _effectContainer;
    private StatsManager _stats;
    private float _sightRange => _stats.GetModifiedStat(EUnitFloatStats.SightRange);

    protected override void OnSpawnSuccess()
    {
        _stats = Container.GetComponent<StatsManager>();
        _effectContainer = Container.GetComponent<EffectContainer>();
        _moveSystem = Container.GetComponent<IMoveSystem>();
        TargetFinder = new TargetFinder(Container.GetComponent<TeamController>(), _sightRange, false);
    }

    protected override void OnEnter()
    {
        if (!Runner.IsServer) return;

        _effectContainer.AddEffect(statMod);
        TargetFinder.Tick();
        _moveSystem.SetTarget(TargetFinder.Target);
    }

    protected override void UniversalTick()
    {
        TargetFinder.Tick();
    }

    protected override void Tick()
    {
        _moveSystem.SetTarget(TargetFinder.Target);
        if (TargetFinder.Target.Hit == null) _stateMachine.Exit(this);
    }

    public override void OnExit()
    {
        _effectContainer.RemoveEffect(statMod);
    }
}
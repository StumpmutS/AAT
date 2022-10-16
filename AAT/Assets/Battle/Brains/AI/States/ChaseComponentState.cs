using Fusion;
using UnityEngine;

public class ChaseComponentState : ComponentState<AiTransitionBlackboard>
{
    [SerializeField] private StatModifier statMod;

    public TargetFinder TargetFinder { get; private set; }
    private IMoveSystem _moveSystem;
    private StatsManager stats;
    private float _sightRange => stats.GetStat(EUnitFloatStats.SightRange);

    protected override void OnSpawnSuccess()
    {
        TargetFinder = new TargetFinder(Container.GetComponent<TeamController>(), _sightRange, false);
        _moveSystem = Container.GetComponent<IMoveSystem>();
        stats = Container.GetComponent<StatsManager>();
    }

    protected override void OnEnter()
    {
        if (!Runner.IsServer) return;

        stats.AddModifier(statMod);
        TargetFinder.Tick();
        _moveSystem.Follow(TargetFinder.Target);
    }

    protected override void UniversalTick()
    {
        TargetFinder.Tick();
    }

    protected override void Tick()
    {
        _moveSystem.Follow(TargetFinder.Target);
        if (TargetFinder.Target.Hit == null) _stateMachine.Exit(this);
    }

    public override void OnExit()
    {
        stats.RemoveModifier(statMod);
    }
}

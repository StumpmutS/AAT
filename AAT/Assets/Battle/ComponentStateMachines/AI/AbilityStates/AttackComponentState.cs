using System;
using System.Collections;
using Fusion;
using UnityEngine;

public class AttackComponentState : AiComponentState
{
    /*
    [Networked(OnChanged = nameof(OnAttackChanged))] private NetworkBool _attack { get; set; }
    public static void OnAttackChanged(Changed<AttackComponentState> changed)
    {
        changed.Behaviour._animation.SetAttack(true);
    }
    
    [Networked(OnChanged = nameof(OnCritChanged))] private NetworkBool _crit { get; set; }
    public static void OnCritChanged(Changed<AttackComponentState> changed)
    {
        changed.Behaviour._animation.SetCrit(true);
    }
    */

    public TargetFinder TargetFinder { get; private set; }
    protected StatsManager Stats;
    protected IAttackSystem _attackSystem;
    protected IMoveSystem _moveSystem;
    private UnitAnimationController _animation;
    
    protected bool _canAttack;
    public StumpTarget Target { get; protected set; }
    private StumpTarget _animTarget;
    
    protected float _damage => Stats.GetModifiedStat(EUnitFloatStats.Damage);
    protected float _critChancePercent => Stats.GetModifiedStat(EUnitFloatStats.CritChancePercent);
    protected float _critMultiplierPercent => Stats.GetModifiedStat(EUnitFloatStats.CritMultiplierPercent);
    private float _attackSpeedPercent => Stats.GetModifiedStat(EUnitFloatStats.AttackSpeedPercent);
    private float _attackRange => Stats.GetModifiedStat(EUnitFloatStats.AttackRange);
    
    public event Action<GameObject, float> OnAttack = delegate {  };
    public event Action<GameObject, float> OnCrit = delegate {  };

    protected override void OnSpawnSuccess()
    {
        _animation = Container.GetComponent<UnitAnimationController>();
        _canAttack = true;
        Stats = Container.GetComponent<StatsManager>();
        TargetFinder = new TargetFinder(Container.GetComponent<TeamController>(), _attackRange, true);
        _moveSystem = Container.GetComponent<IMoveSystem>();
    }

    protected override void OnEnter()
    {
        Target = TargetFinder.Target;
        _moveSystem.Stop();
    }

    protected override void UniversalTick()
    {
        TargetFinder.Tick();
    }

    protected override void Tick()
    {
        Attack();
    }

    protected virtual void Attack(StumpTarget target = null)
    {
        Target = TargetFinder.Target ?? target;
        if (!_canAttack) return;
        
        if (Target == null)
        {
            _stateMachine.Exit(this);
            return;
        }
        
        _attackSystem.CallAttack(target);
    }

    protected IEnumerator StartAttackTimer()
    {
        _canAttack = false;
        yield return new WaitForSeconds(100 / _attackSpeedPercent);
        _canAttack = true;
    }

    public override void OnExit() { }
}
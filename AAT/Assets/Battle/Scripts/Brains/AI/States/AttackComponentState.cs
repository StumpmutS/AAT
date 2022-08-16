using System.Collections;
using Fusion;
using UnityEngine;

public class AttackComponentState : ComponentState
{
    [Networked(OnChanged = nameof(OnAttack))] private NetworkBool _attack { get; set; }
    public static void OnAttack(Changed<AttackComponentState> changed)
    {
        changed.Behaviour._animation.SetAttack(true);
    }
    
    [Networked(OnChanged = nameof(OnCrit))] private NetworkBool _crit { get; set; }
    public static void OnCrit(Changed<AttackComponentState> changed)
    {
        changed.Behaviour._animation.SetCrit(true);
    }
    
    protected TargetFinder _targetFinder;
    protected UnitStatsModifierManager _unitStats;
    private AgentBrain _agentBrain;
    protected IAgent _agent => _agentBrain.CurrentAgent;
    private UnitAnimationController _animation;
    
    protected bool _canAttack;
    protected Component _target;
    
    protected float _damage => _unitStats.GetStat(EUnitFloatStats.Damage);
    protected float _critChancePercent => _unitStats.GetStat(EUnitFloatStats.CritChancePercent);
    protected float _critMultiplierPercent => _unitStats.GetStat(EUnitFloatStats.CritMultiplierPercent);
    private float _attackSpeedPercent => _unitStats.GetStat(EUnitFloatStats.AttackSpeedPercent);

    public override void Spawned()
    {
        _animation = Container.GetComponent<UnitAnimationController>();
        
        if (!Runner.IsServer) return;

        _canAttack = true;
        _targetFinder = Container.GetComponent<TargetFinder>();
        _unitStats = Container.GetComponent<UnitStatsModifierManager>();
        _agentBrain = Container.GetComponent<AgentBrain>();
    }

    public override void Tick()
    {
        base.Tick();
        if (_target == null)
        {
            _target = _targetFinder.AttackTarget;
            if (_target == null)
            {
                _componentStateMachine.Exit(this);
                return;
            }
        }
        CallAttack(_target);
    }

    public virtual void CallAttack(Component target)
    {
        transform.LookAt(target.transform);
        if (!_canAttack) return;
        CheckCrit(target);
        StartCoroutine(StartAttackTimer());
    }

    protected IEnumerator StartAttackTimer()
    {
        _canAttack = false;
        yield return new WaitForSeconds(100 / _attackSpeedPercent);
        _canAttack = true;
    }

    private void CheckCrit(Component target)
    {
        if (Random.Range(0f, 100f) <= _critChancePercent)
        {
            CritAttack(target);
        }
        else
        {
            BaseAttack(target);
        }
    }

    protected void BaseAttack(Component target)
    {
        target.GetComponent<IHealth>().ModifyHealth(-Mathf.Abs(_damage));
        _attack = !_attack;
    }

    protected void CritAttack(Component target)
    {
        target.GetComponent<IHealth>().ModifyHealth(-Mathf.Abs(_damage) * (_critMultiplierPercent / 100));
        _crit = !_crit;
    }

    protected override void OnEnter()
    {
        _target = _targetFinder.SightTarget;
        _agent.EnableAgent(this);
        _agent.ClearDestination();
    }

    public override void OnExit() { }
}

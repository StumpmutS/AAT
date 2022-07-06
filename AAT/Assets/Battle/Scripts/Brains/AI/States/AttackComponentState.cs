using System.Collections;
using UnityEngine;

public class AttackComponentState : ComponentState
{
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
    private float _attackRange => _unitStats.GetStat(EUnitFloatStats.AttackRange);

    protected virtual void Awake()
    {
        _canAttack = true;
        _targetFinder = GetComponent<TargetFinder>();
        _unitStats = GetComponent<UnitStatsModifierManager>();
        _agentBrain = GetComponent<AgentBrain>();
        _animation = GetComponent<UnitAnimationController>();
    }

    public override void Tick()
    {
        base.Tick();
        if (CheckTargetBad())
        {
            _target = _targetFinder.Target;
            if (CheckTargetBad())
            {
                _componentStateMachine.Exit(this);
                return;
            }
        }
        CallAttack(_target);
    }

    private bool CheckTargetBad() => _target == null || Vector3.Distance(_target.transform.position, transform.position) > _attackRange;

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
        _animation?.SetAttack(true);
    }

    protected void CritAttack(Component target)
    {
        target.GetComponent<IHealth>().ModifyHealth(-Mathf.Abs(_damage) * (_critMultiplierPercent / 100));
        _animation?.SetCrit(true);
    }

    public override void OnEnter()
    {
        _target = _targetFinder.Target;
        _agent.EnableAgent(this);
        _agent.ClearDestination();
    }

    public override void OnExit() { }
}

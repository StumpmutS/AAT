using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private DecalImage decalImage;
    [SerializeField] private ColorsData decalColors;

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
        base.Spawned();
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
        CallAttack();
    }

    public virtual void CallAttack(Component target = null)
    {
        _target = _targetFinder.AttackTarget;
        if (target != null) _target = target;
        bool hasTarget = _target != null;
        if (hasTarget)
        {
            Container.transform.LookAt(_target.transform);
        }
        if (!_canAttack) return;
        
        if (!hasTarget)
        {
            _componentStateMachine.Exit(this);
            return;
        }
        CheckCrit();
    }

    protected IEnumerator StartAttackTimer()
    {
        _canAttack = false;
        yield return new WaitForSeconds(100 / _attackSpeedPercent);
        _canAttack = true;
    }

    protected virtual void CheckCrit()
    {
        if (Random.Range(0f, 100f) <= _critChancePercent)
        {
            AnimateCrit();
        }
        else
        {
            AnimateAttack();
        }
        StartCoroutine(StartAttackTimer());
    }

    protected void AnimateAttack()
    {
        _attack = !_attack;
    }

    protected void AnimateCrit()
    {
        _crit = !_crit;
    }

    public void AnimationTriggeredAttack()
    {
        var info = new AttackDecalInfo(decalColors.Colors[0], decalImage.MaxSeverity, _target.transform.position - Container.transform.position, _target.transform.position);
        _target.GetComponent<IHealth>().ModifyHealth(-Mathf.Abs(_damage), decalImage, info);
    }

    public void AnimationTriggeredCrit()
    {
        var info = new AttackDecalInfo(decalColors.Colors[1], decalImage.MaxSeverity, _target.transform.position - Container.transform.position, _target.transform.position);
        _target.GetComponent<IHealth>().ModifyHealth(-Mathf.Abs(_damage) * (_critMultiplierPercent / 100), decalImage, info);
    }

    protected override void OnEnter()
    {
        _target = _targetFinder.SightTarget;
        _agent.EnableAgent(this);
        _agent.ClearDestination();
    }

    public override void OnExit() { }
}

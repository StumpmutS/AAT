using UnityEngine;

public class TransportedComponentState : InteractionComponentState
{
    private UnitStatsModifierManager _stats;
    private TargetFinder _targetFinder;
    private AgentBrain _agentBrain;
    private IAgent _agent => _agentBrain.CurrentAgent;
    private UnitDeathController _death;
    private SelectableController _selectable;
    private AttackComponentState _attackComponentState;

    private SelfOtherStatsData _transportableData;
    private BaseMountableController _mount;
    private bool _checkGroundSubscribed;
    
    private void Awake()
    {
        _stats = GetComponent<UnitStatsModifierManager>();
        _targetFinder = GetComponent<TargetFinder>();
        _agentBrain = GetComponent<AgentBrain>();
        _death = GetComponent<UnitDeathController>();
        _selectable = GetComponent<SelectableController>();
        _transportableData = GetComponent<TransportableData>().SelfOtherStatsData;
    }

    private void Start() => _attackComponentState = GetComponent<AttackComponentState>();

    public void Mount(BaseMountableController mount)
    {
        if (mount == _mount) return;
        _mount = mount;
        _agent.DisableAgent(this);
        transform.position = _mount.transform.position;
        transform.rotation = _mount.transform.rotation;
        transform.parent = _mount.transform;
        _stats.ModifyStats(_mount.MountData.OtherModifier);
        _stats.ModifyStats(_transportableData.SelfModifier);
        _mount.ActivateMounted(_transportableData.OtherModifier);
        _mount.Unit.OnDeath += MountDeathHandler;
    }
    
    private void MountDeathHandler(UnitController notNeeded)
    {
        _death.Die();
    }

    public override void OnEnter()
    {
        _agent.DisableAgent(this);
        InputManager.OnRightClickUp += Demount;
    }

    public override void Tick()
    {
        base.Tick();
        if (_mount is not null)
            CheckAttack();
    }

    private void CheckAttack()
    {
        if (_targetFinder.Target is not null && _attackComponentState is not null)
        {
            _attackComponentState.CallAttack(_targetFinder.Target);
        }
    }

    public override void OnExit()
    {
        _agent.EnableAgent(this);
        InputManager.OnRightClickUp -= Demount;
    }

    private void Demount()
    {
        if (!_selectable.Selected) return;
        var pos = Vector3.zero;
        if (Physics.Raycast(transform.position, -Vector3.up, out var demountHit, LayerManager.Instance.GroundLayer))
        {
            pos = demountHit.point;
        }
        _stats.ModifyStats(_mount.MountData.OtherModifier, false);
        _stats.ModifyStats(_transportableData.SelfModifier, false);
        _mount.DeactivateMounted(_transportableData.OtherModifier);
        _mount.Unit.OnDeath -= MountDeathHandler;
        _mount = null;
        transform.position = pos;
        _agent.EnableAgent(this);
        _agent.Warp(pos);
    }
}

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
    
    public override void Spawned()
    {
        if (!Runner.IsServer) return;

        _stats = Container.GetComponent<UnitStatsModifierManager>();
        _targetFinder = Container.GetComponent<TargetFinder>();
        _agentBrain = Container.GetComponent<AgentBrain>();
        _death = Container.GetComponent<UnitDeathController>();
        _selectable = Container.GetComponent<SelectableController>();
        _transportableData = Container.GetComponent<TransportableData>().SelfOtherStatsData;
    }

    private void Start()
    {
        if (!Runner.IsServer) return;

        _attackComponentState = Container.GetComponent<AttackComponentState>();
    }

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

    protected override void OnEnter()
    {
        _agent.DisableAgent(this);
        BaseInputManager.OnRightClickUp += Demount; //todo get player hit and demount unless enemy in range clicked
    }

    public override void Tick()
    {
        base.Tick();
        if (_mount is not null)
            CheckAttack();
    }

    private void CheckAttack()
    {
        if (_targetFinder.SightTarget != null && _attackComponentState != null)
        {
            _attackComponentState.CallAttack(_targetFinder.SightTarget);
        }
    }

    public override void OnExit()
    {
        _agent.EnableAgent(this);
        BaseInputManager.OnRightClickUp -= Demount;
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

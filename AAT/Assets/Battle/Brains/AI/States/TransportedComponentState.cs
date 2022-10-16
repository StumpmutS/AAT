using UnityEngine;

public class TransportedComponentState : InteractionComponentState
{
    private StatsManager _stats;
    private IAttackSystem _attackSystem;
    private TargetFinder _targetFinder;
    private IMoveSystem _moveSystem;
    private UnitDeathController _death;
    private SelectableController _selectable;

    private SelfOtherStatsData _transportableData;
    private BaseMountableController _mount;
    private bool _checkGroundSubscribed;
    
    public override void Spawned()
    {
        base.Spawned();
        _stats = Container.GetComponent<StatsManager>();
        _targetFinder = new TargetFinder(GetComponent<TeamController>(), _stats.GetStat(EUnitFloatStats.AttackRange), true);
        _moveSystem = Container.GetComponent<IMoveSystem>();
        _death = Container.GetComponent<UnitDeathController>();
        _selectable = Container.GetComponent<SelectableController>();
        _transportableData = Container.GetComponent<TransportableData>().SelfOtherStatsData;
    }

    private void Start()
    {
        _attackSystem = Container.GetComponent<IAttackSystem>();
    }

    public void Mount(BaseMountableController mount)
    {
        if (mount == _mount) return;
        _mount = mount;
        _moveSystem.Disable();
        transform.position = _mount.transform.position;
        transform.rotation = _mount.transform.rotation;
        transform.parent = _mount.transform;
        _stats.AddModifier(_mount.MountData.OtherModifier);
        _stats.AddModifier(_transportableData.SelfModifier);
        //_mount.ActivateMounted(_transportableData.OtherModifier);todo
        _mount.Unit.OnDeath += MountDeathHandler;
    }
    
    private void MountDeathHandler(UnitController notNeeded)
    {
        _death.Die();
    }

    protected override void OnEnter()
    {
        _moveSystem.Disable();
        BaseInputManager.OnRightClickUp += Demount; //todo get player hit and demount unless enemy in range clicked
    }

    protected override void Tick()
    {
        base.Tick();
        _targetFinder.Tick();
        if (_mount is not null)
            CheckAttack();
    }

    private void CheckAttack()
    {
        if (_targetFinder.Target.Hit != null && _attackSystem != null)
        {
            _attackSystem.CallAttack(_targetFinder.Target);
        }
    }

    public override void OnExit()
    {
        _moveSystem.Enable();
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
        _stats.RemoveModifier(_mount.MountData.OtherModifier);
        _stats.RemoveModifier(_transportableData.SelfModifier);
        //_mount.DeactivateMounted(_transportableData.OtherModifier);todo
        _mount.Unit.OnDeath -= MountDeathHandler;
        _mount = null;
        transform.position = pos;
        _moveSystem.Enable();
    }
}

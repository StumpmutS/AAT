using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnitController))]
public class TransportableController : MonoBehaviour
{
    [SerializeField] private TransportableData transportableData;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AIPathfinder AI;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private UnitStatsModifierManager statsMod;
    [SerializeField] private BaseAttackController attackController;

    private UnitController _unitController;
    public UnitController UnitController => _unitController;

    public event Action<TransportableController> OnTransportableSelect = delegate { };
    public event Action<TransportableController> OnTransportableDeselect = delegate { };

    private float attackRange => statsMod.CurrentUnitStatsData.UnitFloatStats[EUnitFloatStats.AttackRange];
    private BaseMountableController _mount = null;
    private bool _mounted;
    private bool _movingToMount;
    private bool _checkGroundSubscribed;
    private bool _selected = false;

    private void Awake()
    {
        var AIOverride = AI as AIPlayerOverrideController;
        if (AIOverride != null) AIOverride.OnReroute += RerouteHandler;
        _unitController = GetComponent<UnitController>();
        _unitController.OnSelect += Select;
        _unitController.OnDeselect += Deselect;
    }

    private void Start()
    {
        MountManager.Instance.AddTransportable(this);
    }

    private void Select()
    {
        OnTransportableSelect.Invoke(this);
        if (_mounted) SubscribeCheckGround(true);
        _selected = true;
    }

    private void Deselect()
    {
        OnTransportableDeselect.Invoke(this);
        SubscribeCheckGround(false);
        _selected = false;
    }

    public void BeginMountProcess(BaseMountableController mount)
    {
        if (mount == _mount) return;
        
        if (AI != null) AI.Deactivate();
        _mount = mount;
        _movingToMount = true;
        InputManager.OnUpdate += CheckMountRange;
    }

    private void CheckMountRange()
    {
        if (agent.remainingDistance < _mount.ReturnData().MountRange)
        {
            _movingToMount = false;
            InputManager.OnUpdate -= CheckMountRange;
            Mount();
        } 
        else
        {
            agent.SetDestination(_mount.transform.position);
        }
    }

    private void Mount()
    {
        _mounted = true;
        agent.enabled = false;
        transform.position = _mount.transform.position;
        transform.rotation = _mount.transform.rotation;
        transform.parent = _mount.transform;
        InputManager.OnUpdate += CheckAttack;
        if (_selected) SubscribeCheckGround(true);
        _unitController.ModifyStats(_mount.ReturnData().MountedUnitModifier);
        _unitController.ModifyStats(transportableData.SelfStatsAlter);
        _mount.ActivateMounted(transportableData.MountStatsAlter);
    }

    private void CheckAttack()
    {
        if (AI.CheckRange(attackRange, out var target))
        {
            attackController.CallAttack(target);
        }
    }

    private void CheckGround()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(transform.position, -Vector3.up, out var demountHit, groundLayer) && Physics.Raycast(ray))
        {
            SubscribeCheckGround(false);
            Demount(demountHit.point);
        }
    }

    private void Demount(Vector3 pos)
    {
        print("demount");
        _unitController.ModifyStats(_mount.ReturnData().MountedUnitModifier, false);
        _unitController.ModifyStats(transportableData.SelfStatsAlter, false);
        _mount.DeactivateMounted(transportableData.MountStatsAlter);
        _mount = null;
        _mounted = false;
        transform.position = pos;
        agent.enabled = true;
        agent.Warp(pos);
        if (AI != null) AI.Activate();
        var AIOverride = AI as AIPlayerOverrideController;
        if (AIOverride != null) AIOverride.SetTargetDestination();
    }

    private void RerouteHandler()
    {
        if (_movingToMount) InputManager.OnUpdate -= CheckMountRange;
    }

    private void SubscribeCheckGround(bool subscribe)
    {
        if (subscribe && !_checkGroundSubscribed)
        {
            _checkGroundSubscribed = true;
            InputManager.OnRightClick += CheckGround;
        }
        else if (_checkGroundSubscribed)
        {
            _checkGroundSubscribed = false;
            InputManager.OnRightClick -= CheckGround;
        }
    }
}

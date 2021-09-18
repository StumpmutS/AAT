using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnitController))]
public class TransportableController : MonoBehaviour
{
    [SerializeField] private AIPathfinder AI;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private UnitStatsModifierManager statsMod;
    [SerializeField] private BaseAttackController attackController;

    private UnitController unitController;
    public UnitController UnitController => unitController;

    public event Action<TransportableController> OnTransportableSelect = delegate { };
    public event Action<TransportableController> OnTransportableDeselect = delegate { };

    private float attackRange => statsMod.CurrentUnitStatsData.AttackRange;
    private BaseMountableController _mount = null;
    private bool _movingToMount;

    private void Awake()
    {
        if (AI is AIPlayerOverrideController) GetComponent<AIPlayerOverrideController>().OnReroute += RerouteHandler;
        unitController = GetComponent<UnitController>();
        unitController.OnSelect += Select;
        unitController.OnDeselect += Deselect;
    }

    private void Start()
    {
        MountManager.Instance.AddTransportable(this);
    }

    private void Select()
    {
        OnTransportableSelect.Invoke(this);
    }

    private void Deselect()
    {
        OnTransportableDeselect.Invoke(this);
    }

    public void BeginMountProcess(BaseMountableController mount)
    {
        if (mount != _mount)
        {
            if (AI != null) AI.Deactivate();
            _mount = mount;
            _movingToMount = true;
            InputManager.OnUpdate += CheckMountRange;
        }
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
        agent.enabled = false;
        transform.position = _mount.transform.position;
        transform.rotation = _mount.transform.rotation;
        transform.parent = _mount.transform;
        InputManager.OnUpdate += CheckAttack;
    }

    private void CheckAttack()
    {
        if (AI.CheckRange(attackRange, out var target))
        {
            attackController.CallAttack(target);
        }
    }

    private void RerouteHandler()
    {
        if (_movingToMount) InputManager.OnUpdate -= CheckMountRange;
    }
}

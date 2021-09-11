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
    private MountableController _mount = null;
    private Transform _mountPoint = null;

    private void Awake()
    {
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

    public void BeginMountProcess(MountableController mount, int index)
    {
        var newMountPoint = mount.MountablePoints[index];
        if (mount == _mount && newMountPoint == _mountPoint) return;
        if (AI != null) AI.Deactivate();
        _mount = mount;
        _mountPoint = mount.MountablePoints[index];
        InputManager.OnUpdate += CheckMountRange;
    }

    private void CheckMountRange()
    {
        if (agent.remainingDistance < _mount.MountData.MountRange)
        {
            InputManager.OnUpdate -= CheckMountRange;
            Mount();
        } 
        else
        {
            agent.SetDestination(_mountPoint.position);
        }
    }

    private void Mount()
    {
        agent.enabled = false;
        transform.position = _mountPoint.position;
        transform.rotation = _mountPoint.rotation;
        transform.parent = _mountPoint;
        InputManager.OnUpdate += CheckAttack;
    }

    private void CheckAttack()
    {
        if (AI.CheckRange(attackRange, out var target))
        {
            attackController.CallAttack(target);
        }
    }
}

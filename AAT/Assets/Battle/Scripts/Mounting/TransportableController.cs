using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnitController), typeof(AATAgentController))]
public class TransportableController : MonoBehaviour //TODO:
{
    [SerializeField] private SelfOtherStatsData transportableData;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AIPathfinder AI;
    [SerializeField] private UnitStatsModifierManager statsMod;
    [SerializeField] private BaseAttackController attackController;

    private UnitController _unit;

    private float _attackRange => statsMod.CurrentUnitStatsData[EUnitFloatStats.AttackRange];        
    private AATAgentController _agent;
    private BaseMountableController _mount;
    private bool _checkGroundSubscribed;
    private bool _selected;
    
    private void Awake()
    {
        _unit = GetComponent<UnitController>();
        _agent = GetComponent<AATAgentController>();
        _unit.OnSelect += Select;
        _unit.OnDeselect += Deselect;
    }

    private void Select()
    {
        if (_mount != null) SubscribeCheckGround();
        _selected = true;
    }

    private void Deselect()
    {
        UnsubscribeCheckGround();
        _selected = false;
    }

    public void Mount(BaseMountableController mount)
    {
        if (mount == _mount) return;
        if (AI != null) AI.Deactivate();
        _mount = mount;
        _agent.DisableAgent(this);
        transform.position = _mount.transform.position;
        transform.rotation = _mount.transform.rotation;
        transform.parent = _mount.transform;
        InputManager.OnUpdate += CheckAttack;
        if (_selected) SubscribeCheckGround();
        _unit.ModifyStats(_mount.MountData.OtherModifier);
        _unit.ModifyStats(transportableData.SelfModifier);
        _mount.ActivateMounted(transportableData.OtherModifier);
    }

    private void CheckAttack()
    {
        if (AI.CheckRange(_attackRange, out var target))
        {
            attackController.CallAttack(target);
        }
    }

    private void CheckGround()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(transform.position, -Vector3.up, out var demountHit, groundLayer) && Physics.Raycast(ray))
        {
            UnsubscribeCheckGround();
            Demount(demountHit.point);
        }
    }

    private void Demount(Vector3 pos)
    {
        print("demount");
        _unit.ModifyStats(_mount.MountData.OtherModifier, false);
        _unit.ModifyStats(transportableData.SelfModifier, false);
        _mount.DeactivateMounted(transportableData.OtherModifier);
        _mount = null;
        transform.position = pos;
        _agent.EnableAgent(this);
        _agent.Warp(pos);
        if (AI != null) AI.Activate();
        if (AI is AIPlayerOverrideController AIOverride) AIOverride.SetTargetDestination();
        InputManager.OnUpdate -= CheckAttack;
    }

    private void SubscribeCheckGround()
    {
        if (_checkGroundSubscribed) return;
        _checkGroundSubscribed = true;
        InputManager.OnRightClickDown += CheckGround;
    }

    private void UnsubscribeCheckGround()
    {
        if (!_checkGroundSubscribed) return;
        _checkGroundSubscribed = false;
        InputManager.OnRightClickDown -= CheckGround;
    }
}

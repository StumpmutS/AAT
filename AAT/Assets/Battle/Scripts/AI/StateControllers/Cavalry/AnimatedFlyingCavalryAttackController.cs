using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedFlyingCavalryAttackController : AnimatedCavalryAttackController
{
    [SerializeField] private float flyBackSpeed;

    private Vector3 _originalPosition;

    private void Start()
    {
        _originalPosition = transform.position;
    }

    public override void CallAttack(GameObject target)
    {
        base.CallAttack(target);
        AI.Deactivate();
        _agent.enabled = false;
    }

    protected override void AddSpeed()
    {
        float addSpeed =  ((moveSpeed * chaseSpeedPercent / 100) - moveSpeed) * Time.deltaTime;
        unitDataManager.ModifyFloatStat(EUnitFloatStats.MovementSpeed, addSpeed);
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, addSpeed);
        transform.LookAt(_target.transform);
        _chargeSpeedChange += addSpeed;
    }

    protected override void EndCharge()
    {
        base.EndCharge();
        InputManager.OnUpdate += FlyBack;
    }

    private void FlyBack()
    {
        Vector3 pos = transform.position;
        transform.position = Vector3.MoveTowards(pos, pos + new Vector3(pos.x, _originalPosition.y, pos.z), flyBackSpeed * Time.deltaTime);
        if (transform.position.y >= _originalPosition.y)
        {
            transform.position = new Vector3(transform.position.x, _originalPosition.y, transform.position.z);
            InputManager.OnUpdate -= FlyBack;
            AI.Activate();
            _agent.enabled = true;
        }
    }
}

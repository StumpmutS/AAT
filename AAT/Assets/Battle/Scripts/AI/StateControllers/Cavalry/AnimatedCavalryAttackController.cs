using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimatedCavalryAttackController : AnimatedAttackController
{
    [SerializeField] private float chargeEndDistance;
    
    protected float chaseSpeedPercent => unitDataManager.CurrentUnitStatsData.UnitFloatStats[EUnitFloatStats.ChaseSpeedPercentMultiplier];
    protected float moveSpeed => unitDataManager.CurrentUnitStatsData.UnitFloatStats[EUnitFloatStats.MovementSpeed];

    protected UnitController _target;
    private float _chargeTime;
    protected float _chargeSpeedChange;
    
    public override void CallAttack(GameObject target)
    {
        if (!_canAttack) return;
        _chargeTime = 0;
        _target = target.GetComponent<UnitController>();
        InputManager.OnUpdate += Charge;
        ExecuteStartAttackTimer();
    }

    protected virtual void ExecuteStartAttackTimer()
    {
        StartCoroutine(StartAttackTimer());
    }

    protected override void CheckCrit(UnitController target)
    {
        if (_chargeTime > (100 - critChancePercent) / 10)
        {
            CritAttack(target);
        }
        else
        {
            BaseAttack(target);
        }
    }
    
    protected override void CritAttack(UnitController target)
    {
        unitAnimation.SetCrit(true);
        target.GetComponent<IHealth>().ModifyHealth(-(Mathf.Abs(damage)) * critMultiplierPercent / 100);
    }

    private void Charge()
    {
        _chargeTime += Time.deltaTime;
        AddSpeed();
        if (Vector3.Distance(transform.position, _target.transform.position) < chargeEndDistance)
        {
            EndCharge();
        }
    }

    protected virtual void AddSpeed()
    {
        float addSpeed =  ((moveSpeed * chaseSpeedPercent / 100) - moveSpeed) * Time.deltaTime;
        unitDataManager.ModifyFloatStat(EUnitFloatStats.MovementSpeed, addSpeed);
        _agent.speed += addSpeed;
        _chargeSpeedChange += addSpeed;
    }

    protected virtual void EndCharge()
    {
        InputManager.OnUpdate -= Charge;
        CheckCrit(_target);
        _target = null;
        unitDataManager.ModifyFloatStat(EUnitFloatStats.MovementSpeed, -_chargeSpeedChange);
        _chargeSpeedChange = 0;
        if (_agent != null)
            _agent.speed = moveSpeed;
    }
}

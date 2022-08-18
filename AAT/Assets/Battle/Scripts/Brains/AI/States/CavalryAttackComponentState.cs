using UnityEngine;

public class CavalryAttackComponentState : AttackComponentState
{
    protected float _chaseSpeedPercent => _unitStats.GetStat(EUnitFloatStats.ChaseSpeedPercentMultiplier);
    protected float _moveSpeed => _unitStats.GetStat(EUnitFloatStats.MovementSpeed);
    private float _chargeEndDistance => _unitStats.GetStat(EUnitFloatStats.ChargeEndDistance);

    private bool _charging;
    private float _chargeTime;
    protected float _chargeSpeedChange;

    public override void CallAttack(Component target = null)
    {
        if (!_canAttack || _charging) return;
        _charging = true;
        _chargeTime = 0;
        ExecuteStartAttackTimer();
    }

    public override void Tick()
    {
        if (!_charging) base.Tick();
        Charge();
    }

    private void Charge()
    {
        if (!_charging) return;
        _chargeTime += Runner.DeltaTime;
        AddSpeed();
        _agent.SetDestination(_target.transform.position);
        if (Vector3.Distance(transform.position, _target.transform.position) < _chargeEndDistance || _target == null)
        {
            EndCharge();
        }
    }

    protected virtual void AddSpeed()
    {
        float addSpeed =  (_moveSpeed * _chaseSpeedPercent / 100 - _moveSpeed) * Runner.DeltaTime;
        _unitStats.ModifyFloatStat(EUnitFloatStats.MovementSpeed, addSpeed);
        _chargeSpeedChange += addSpeed;
    }

    protected virtual void EndCharge()
    {
        _charging = false;
        CheckCrit();
        _target = null;
        _unitStats.ModifyFloatStat(EUnitFloatStats.MovementSpeed, -_chargeSpeedChange);
        _chargeSpeedChange = 0;
    }

    private void ExecuteStartAttackTimer()
    {
        StartCoroutine(StartAttackTimer());
    }

    protected override void CheckCrit()
    {
        if (_chargeTime > (100 - _critChancePercent) / 10)
        {
            AnimateCrit();
        }
        else
        {
            AnimateAttack();
        }
    }
}

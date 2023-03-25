using UnityEngine;

public class CavalryAttackComponentState : AttackComponentState
{
    [SerializeField] private float chargeEndDistance;
    
    protected float _chaseSpeedPercent => Stats.GetModifiedStat(EUnitFloatStats.ChaseSpeedPercentMultiplier);
    protected float _moveSpeed => Stats.GetModifiedStat(EUnitFloatStats.MovementSpeed);

    private bool _charging;
    private float _chargeTime;
    protected float _chargeSpeedChange;

    protected override void Attack(StumpTarget target = null)
    {
        if (!_canAttack || _charging) return;
        _charging = true;
        _chargeTime = 0;
        ExecuteStartAttackTimer();
    }

    protected override void Tick()
    {
        if (!_charging) base.Tick();
        Charge();
    }

    private void Charge()
    {
        if (!_charging) return;
        _chargeTime += Runner.DeltaTime;
        AddSpeed();
        _moveSystem.SetTarget(Target);
        if (Vector3.Distance(transform.position, Target.Hit.transform.position) < chargeEndDistance || Target == null)
        {
            EndCharge();
        }
    }

    protected virtual void AddSpeed()
    {
        float addSpeed =  (_moveSpeed * _chaseSpeedPercent / 100 - _moveSpeed) * Runner.DeltaTime;
        Stats.ModifyFloatStat(EUnitFloatStats.MovementSpeed, addSpeed);
        _chargeSpeedChange += addSpeed;
    }

    protected virtual void EndCharge()
    {
        _charging = false;
        //CheckCrit();
        Target = null;
        Stats.ModifyFloatStat(EUnitFloatStats.MovementSpeed, -_chargeSpeedChange);
        _chargeSpeedChange = 0;
    }

    private void ExecuteStartAttackTimer()
    {
        StartCoroutine(StartAttackTimer());
    }

    /*protected override void CheckCrit()todo
    {
        if (_chargeTime > (100 - _critChancePercent) / 10)
        {
            AnimateCrit();
        }
        else
        {
            AnimateAttack();
        }
    }*/
}

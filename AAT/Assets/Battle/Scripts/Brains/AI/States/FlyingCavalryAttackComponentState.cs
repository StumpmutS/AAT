using UnityEngine;

public class FlyingCavalryAttackComponentState : CavalryAttackComponentState
{
    protected override void AddSpeed()
    {
        float addSpeed =  (_moveSpeed * _chaseSpeedPercent / 100 - _moveSpeed) * Time.deltaTime;
        _unitStats.ModifyFloatStat(EUnitFloatStats.MovementSpeed, addSpeed);
        transform.LookAt(_target.transform);
        _chargeSpeedChange += addSpeed;
    }

    protected override void EndCharge()
    {
        base.EndCharge();
        _agent.SetDestination(new Vector3(transform.position.x, 0, transform.position.y));
    }
}

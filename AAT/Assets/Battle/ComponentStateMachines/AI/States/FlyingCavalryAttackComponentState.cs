using UnityEngine;

public class FlyingCavalryAttackComponentState : CavalryAttackComponentState
{
    protected override void AddSpeed()
    {
        float addSpeed =  (_moveSpeed * _chaseSpeedPercent / 100 - _moveSpeed) * Runner.DeltaTime;
        Stats.ModifyFloatStat(EUnitFloatStats.MovementSpeed, addSpeed);
        _chargeSpeedChange += addSpeed;
    }

    protected override void EndCharge()
    {
        base.EndCharge();
        _moveSystem.SetTarget(new StumpTarget(null, new Vector3(transform.position.x, 0, transform.position.y)));
    }
}

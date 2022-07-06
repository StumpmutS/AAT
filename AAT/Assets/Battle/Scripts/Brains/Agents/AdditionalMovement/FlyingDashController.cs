using System;
using UnityEngine;
using Utility.Scripts;

public class FlyingDashController : DashController
{
    private UnitStatsModifierManager _stats;
    private float _turnSpeed => _stats.GetStat(EUnitFloatStats.TurnSpeed);

    private Vector3 _rotationTarget;
    
    protected override void Awake()
    {
        base.Awake();
        _stats = GetComponent<UnitStatsModifierManager>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_rotationTarget, 1);
    }

    public override void Dash(Vector3 distance, float speed)
    {
        var forward0Y = transform.forward * distance.z;
        forward0Y.y = 0;
        _rotationTarget = transform.position + transform.right * distance.x + transform.up * distance.y + forward0Y;
        base.Dash(distance, speed);
    }

    protected override void SetPosition(Vector3 pos)
    {
        base.SetPosition(pos);
        transform.RotateTowardsOnX(_rotationTarget, _turnSpeed, out _);
        transform.RotateTowardsOnY(_rotationTarget, _turnSpeed, out _);
    }
}
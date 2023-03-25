using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

public class DashComponentState : AbilityComponentState
{
    [SerializeField] private List<Vector3> dashPoints;
    [SerializeField] private float dashSpeed;

    private List<Vector3> _localPoints = new();
    private Vector3 _origPosition;
    private float _currentBezierValue;

    protected override void OnEnter()
    {
        base.OnEnter();
        Configure();
    }

    public virtual void Configure()
    {
        _localPoints = dashPoints.Select(p => p.x * transform.right + p.y * transform.up + p.z * transform.forward).ToList();
        _currentBezierValue = 0;
        _origPosition = transform.position;
    }

    protected override void Tick()
    {
        base.Tick();
        Dash();
    }

    private void Dash()
    {
        _currentBezierValue += dashSpeed * Runner.DeltaTime;
        transform.position = _origPosition + StumpBezierHelpers.SamplePoint(_localPoints, _currentBezierValue);
        transform.forward = StumpBezierHelpers.SampleTangent(_localPoints, _currentBezierValue);
        if (_currentBezierValue >= 1) EndDash();
    }

    public void EndDash()
    {
        _stateMachine.Exit(this);
    }

    private void OnDestroy()
    {
        EndDash();
    }
}

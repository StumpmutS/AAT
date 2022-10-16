using System;
using Fusion;
using UnityEngine;

public abstract class TimedComponentState<T> : ComponentState<T> where T : TransitionBlackboard
{
    [SerializeField] private float time;

    [Networked] private TickTimer _timer { get; set; }
    
    protected override void OnSpawnSuccess() { }

    protected override void OnEnter()
    {
        StartTimer();
    }

    private void StartTimer()
    {
        _timer = TickTimer.CreateFromSeconds(Runner, time);
    }

    protected override void Tick()
    {
        if (_timer.Expired(Runner)) _stateMachine.Exit(this);
    }

    public override void OnExit() { }
}
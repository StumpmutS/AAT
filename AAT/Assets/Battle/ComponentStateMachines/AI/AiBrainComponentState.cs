using System;
using Fusion;
using UnityEngine;

public abstract class AiBrainComponentState : AiComponentState, IBrain<AiTransitionBlackboard>
{
    protected ComponentStateMachine<AiTransitionBlackboard> _componentOwnedStateMachine;

    protected override void OnSpawnSuccess()
    {
        _componentOwnedStateMachine = new ComponentStateMachine<AiTransitionBlackboard>(GetBlackboard());
    }

    public AiTransitionBlackboard GetBlackboard() => _brain.GetBlackboard();

    public ComponentState<AiTransitionBlackboard> AddOrGetState(ComponentState<AiTransitionBlackboard> componentState)
    {
        return Container.AddOrGetComponentState(componentState, this, _componentOwnedStateMachine);
    }

    protected override void Tick()
    {
        _componentOwnedStateMachine.Tick();
    }
}
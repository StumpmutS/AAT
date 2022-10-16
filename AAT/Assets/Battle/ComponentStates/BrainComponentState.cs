using System;
using UnityEngine;

public abstract class BrainComponentState<T> : ComponentState<T>, IBrain<T> where T : TransitionBlackboard
{
    [SerializeField] private NetworkComponentStateContainer<T> ownContainer;

    protected ComponentStateMachine<T> _componentOwnedStateMachine;

    private void Awake()
    {
        _componentOwnedStateMachine = new ComponentStateMachine<T>(GetBlackboard());
    }

    public T GetBlackboard() => _brain.GetBlackboard();

    public ComponentState<T> AddOrGetState(ComponentState<T> componentState)
    {
        return ownContainer.AddOrGetComponentState(componentState, this, _stateMachine);
    }
}
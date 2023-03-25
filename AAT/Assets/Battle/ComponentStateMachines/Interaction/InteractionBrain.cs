using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class InteractionBrain : SimulationBehaviour, ISpawned, IBrain<InteractionTransitionBlackboard>
{
    [SerializeField] private InteractionNetworkComponentStateContainer container;
    [SerializeField] private EmptyInteractionComponentState emptyInteractionState;

    private ComponentStateMachine<InteractionTransitionBlackboard> _stateMachine;
    private InteractionTransitionBlackboard _blackboard;
    private Action _finishedCallback;

    public void Spawned()
    {
        _blackboard = new InteractionTransitionBlackboard();
        _stateMachine = new ComponentStateMachine<InteractionTransitionBlackboard>(_blackboard);
        var emptyState = AddOrGetState(emptyInteractionState);
        _stateMachine.AddAnyTransition(emptyState, _ => false, true);
        _stateMachine.ActivateDefault();
    }

    public bool TrySetInteraction(InteractionComponentState state, Action finishedCallback)
    {
        var empty = AddOrGetState(emptyInteractionState);
        var interactionState = (InteractionComponentState) AddOrGetState(state);
        if (!_stateMachine.Exit(empty, interactionState)) return false;

        _finishedCallback = finishedCallback;
        GetBlackboard().Interacting = true;
        interactionState.OnInteractionFinished += Exit;
        return true;
    }

    private void Exit(InteractionComponentState state)
    {
        _finishedCallback?.Invoke();
        state.OnInteractionFinished -= Exit;
        GetBlackboard().Interacting = false;
        _stateMachine.Exit(state, AddOrGetState(emptyInteractionState));
    }

    public InteractionTransitionBlackboard GetBlackboard() => _blackboard;

    public ComponentState<InteractionTransitionBlackboard> AddOrGetState(ComponentState<InteractionTransitionBlackboard> componentState)
    {
        return container.AddOrGetComponentState(componentState, this, _stateMachine);
    }
}
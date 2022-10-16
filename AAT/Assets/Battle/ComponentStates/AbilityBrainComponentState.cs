using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBrainComponentState : BrainComponentState<AiTransitionBlackboard>
{
    [SerializeField] private EmptyAiComponentState emptyAiState;
    
    public void SetAbility(ComponentState<AiTransitionBlackboard> state)
    {
        var empty = AddOrGetState(emptyAiState);
        var abilityState = AddOrGetState(state);
        if (!_componentOwnedStateMachine.Exit(empty, abilityState))
        {
            throw new NotImplementedException();
        }

        GetBlackboard().AbilityReady = false;
    }

    protected override void OnSpawnSuccess()
    {
        _componentOwnedStateMachine.AddAnyTransition(emptyAiState, _ => false, true);
    }

    protected override void OnEnter() { }

    protected override void Tick()
    {
        _componentOwnedStateMachine.Tick();
    }

    public override void OnExit()
    {
        _componentOwnedStateMachine.ActivateDefault();
    }
}
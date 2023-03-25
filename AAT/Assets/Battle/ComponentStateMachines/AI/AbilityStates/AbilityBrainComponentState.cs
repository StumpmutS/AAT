using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBrainComponentState : AiBrainComponentState
{
    [SerializeField] private EmptyAiComponentState emptyAiState;

    protected override void OnSpawnSuccess()
    {
        base.OnSpawnSuccess();
        var state = AddOrGetState(emptyAiState);
        _componentOwnedStateMachine.AddAnyTransition(state, _ => false, true);
    }

    public void SetAbility(AbilityComponentState state, StumpTarget target)
    {
        var empty = AddOrGetState(emptyAiState);
        var abilityState = (AbilityComponentState) AddOrGetState(state);
        abilityState.SetTarget(target);
        if (!_componentOwnedStateMachine.Exit(empty, abilityState))
        {
            Debug.LogError($"could not exit from {empty.gameObject.name} to {abilityState.gameObject.name}");
        }

        GetBlackboard().AbilityInUse = true;
        abilityState.OnStateFinished += Exit;
    }

    private void Exit()
    {
        GetBlackboard().AbilityInUse = false;
    }

    protected override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void SetValuesFrom(ComponentState<AiTransitionBlackboard> componentState)
    {
        base.SetValuesFrom(componentState);
        emptyAiState = ((AbilityBrainComponentState) componentState).emptyAiState;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Interactable Ready Transition")]
public class InteractableReadyTransition : Transition<TransitionBlackboard>
{
    private Dictionary<UnitController, MovementInteractComponentState> _unitStates = new();

    public override bool Decision(TransitionBlackboard transitionBlackboard)
    {
        throw new NotImplementedException();
        // MovementInteractComponentState componentState;
        // if (!_unitStates.TryGetValue(unit, out componentState))
        // {
        //     componentState = unit.GetComponent<MovementInteractComponentState>();
        //     _unitStates[unit] = componentState;
        // }
        //
        // return componentState._currentInteractable != null;
        return false;
    }
}

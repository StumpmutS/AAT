using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Interactable Ready Transition")]
public class InteractableReadyTransition : Transition
{
    private Dictionary<UnitController, MovementInteractComponentState> _unitStates = new();

    public override bool Decision(UnitController unit)
    {
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

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Interactable Ready Transition")]
public class InteractableReadyTransition : Transition
{
    private Dictionary<UnitController, MovementInteractOverrideComponentState> _unitStates = new();

    public override bool Decision(UnitController unit)
    {
        MovementInteractOverrideComponentState componentState;
        if (!_unitStates.TryGetValue(unit, out componentState))
        {
            componentState = unit.GetComponent<MovementInteractOverrideComponentState>();
            _unitStates[unit] = componentState;
        }

        return componentState.CurrentInteractable != null;
    }
}

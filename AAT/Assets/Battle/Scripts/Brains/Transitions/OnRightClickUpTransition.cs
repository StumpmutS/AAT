using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/On Right Click Up Transition")]
public class OnRightClickUpTransition : Transition
{
    public override bool Decision(UnitController unit)
    {
        return unit.Selected && Input.GetMouseButtonUp(1); //TODO: will change when network input
    }
}

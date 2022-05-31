using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Always Transition")]
public class AlwaysTransition : Transition
{
    public override bool Decision(UnitController unit) => true;
}

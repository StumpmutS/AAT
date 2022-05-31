using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Never Transition")]
public class NeverTransition : Transition
{
    public override bool Decision(UnitController unit) => false;
}

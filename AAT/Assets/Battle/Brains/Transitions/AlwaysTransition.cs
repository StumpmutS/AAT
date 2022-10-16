using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Always Transition")]
public class AlwaysTransition : Transition<TransitionBlackboard>
{
    public override bool Decision(TransitionBlackboard _) => true;
}

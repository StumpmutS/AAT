using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Never Transition")]
public class NeverTransition : Transition<TransitionBlackboard>
{
    public override bool Decision(TransitionBlackboard _) => false;
}

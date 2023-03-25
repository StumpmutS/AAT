using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/AI Never Transition")]
public class AiNeverTransition : AiTransition
{
    public override bool Decision(AiTransitionBlackboard _) => false;
}

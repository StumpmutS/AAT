using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/AI Always Transition")]
public class AiAlwaysTransition : AiTransition
{
    public override bool Decision(AiTransitionBlackboard _) => true;
}

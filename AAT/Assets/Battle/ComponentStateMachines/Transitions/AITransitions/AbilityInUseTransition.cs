using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Ability In Use")]
public class AbilityInUseTransition : AiTransition
{
    public override bool Decision(AiTransitionBlackboard transitionBlackboard)
    {
        return transitionBlackboard.AbilityInUse;
    }
}
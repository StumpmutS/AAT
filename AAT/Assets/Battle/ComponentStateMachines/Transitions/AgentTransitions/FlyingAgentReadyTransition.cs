using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Flying Agent")]
public class FlyingAgentReadyTransition : AgentTransition
{
    public override bool Decision(AgentTransitionBlackboard transitionBlackboard)
    {
        return transitionBlackboard.FlyingAgentReady;
    }
}
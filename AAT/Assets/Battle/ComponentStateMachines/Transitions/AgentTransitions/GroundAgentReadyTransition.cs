using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Ground Agent")]
public class GroundAgentReadyTransition : AgentTransition
{
    public override bool Decision(AgentTransitionBlackboard transitionBlackboard)
    {
        return transitionBlackboard.GroundAgentReady;
    }
}
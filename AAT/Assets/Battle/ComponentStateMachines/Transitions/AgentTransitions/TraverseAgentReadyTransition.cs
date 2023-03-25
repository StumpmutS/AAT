using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Traverse Agent")]
public class TraverseAgentReadyTransition : AgentTransition
{
    public override bool Decision(AgentTransitionBlackboard transitionBlackboard)
    {
        return transitionBlackboard.TraverseAgentReady;
    }
}
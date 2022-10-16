using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBrain : Brain<AgentTransitionBlackboard>
{
    protected override AgentTransitionBlackboard InitBlackboard() => new AgentTransitionBlackboard();
    
    public IAgent CurrentAgent => (IAgent) _componentStateMachine.CurrentComponentState;
}
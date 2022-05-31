using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBrain : Brain
{
    public IAgent CurrentAgent => (IAgent) ComponentStateMachine.CurrentComponentState;
}

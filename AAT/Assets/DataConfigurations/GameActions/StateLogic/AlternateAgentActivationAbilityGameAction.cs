using System;
using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Game Actions/State Logic/Alternate Agent Activation Game Action")]
public class AlternateAgentActivationAbilityGameAction : AbilityGameAction
{
    [Tooltip("Must be inheritor of IAgent")]
    [SerializeField] private TypeReference agentStateType1;
    [Tooltip("Must be inheritor of IAgent")]
    [SerializeField] private TypeReference agentStateType2;

    public override void PerformAction(GameActionInfo info)
    {
        if (TryActivate(info.MainCaller, agentStateType1.TargetType)) return;
        TryActivate(info.MainCaller, agentStateType2.TargetType);
    }

    private bool TryActivate(Component component, Type type)
    {
        if (!component.TryGetComponent<NetworkComponentStateContainer<AgentTransitionBlackboard>>(out var container))
        {
            Debug.LogError("unit does not have a network component state container");
            return false;
        }
        
        if (!container.TryGetComponentState(type, out var foundState))
        {
            Debug.LogError($"unit does not have an agent of type {type.Name}");
            return false;
        }
        
        var iAgent = (IAgent) foundState;
        if (iAgent.IsActive()) return false;
        iAgent.Activate();
        return true;
    }
}

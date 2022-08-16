using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Alternate Agent Activation Ability Component")]
public class AlternateAgentActivationAbilityComponent : AbilityComponent
{
    [Tooltip("Must be inheritor of IAgent")]
    [SerializeField] private ComponentState agent1;
    [Tooltip("Must be inheritor of IAgent")]
    [SerializeField] private ComponentState agent2;

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        if (TryActivate(unit, agent1)) return;
        TryActivate(unit, agent2);
    }

    private bool TryActivate(UnitController unit, ComponentState agent)
    {
        if (!unit.TryGetComponent<NetworkStateComponentContainer>(out var container))
        {
            Debug.LogError("unit does not have a network component state container");
            return false;
        }
        
        if (!container.TryGetComponentState(agent, out var foundState))
        {
            Debug.LogError($"unit does not have an agent of type {agent.GetType().Name}");
            return false;
        }
        
        var iAgent = (IAgent) foundState;
        if (iAgent.IsActive()) return false;
        iAgent.Activate();
        return true;
    }
}

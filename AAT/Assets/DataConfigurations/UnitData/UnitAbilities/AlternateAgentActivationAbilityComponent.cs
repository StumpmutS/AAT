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
        if (!unit.TryGetComponent(agent.GetType(), out var agentComponent)) return false;
        var iAgent = (IAgent) agentComponent;
        if (iAgent.IsActive()) return false;
        iAgent.Activate();
        return true;
    }
}

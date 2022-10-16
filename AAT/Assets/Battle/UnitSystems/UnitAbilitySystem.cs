using UnityEngine;

public class UnitAbilitySystem : MonoBehaviour, IAbilitySystem
{
    [SerializeField] private NetworkComponentStateContainer<AiTransitionBlackboard> container;
    
    public bool AbilityReady() => false;

    public void PrepareAbility(UnitAbilityDataInfo ability)
    {
        throw new System.NotImplementedException();
    }

    public void CastAbility(UnitAbilityDataInfo ability)
    {
        if (container.TryGetComponentState(typeof(AbilityBrainComponentState), out var state))
        {
            Debug.LogError("Ability Brain not found on the state container");
            return;
        }

        ((AbilityBrainComponentState) state).SetAbility(ability.StatePrefab);
    }
}
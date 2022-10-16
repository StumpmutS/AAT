using UnityEngine;

public class BlackboardAbilityReadySetter : BlackboardSetter<AiTransitionBlackboard>
{
    [SerializeField] private GameObject abilitySystem;
    
    private IAbilitySystem _abilitySystem;

    private void Awake()
    {
        _abilitySystem = abilitySystem.GetComponent<IAbilitySystem>();
    }

    public override void FixedUpdateNetwork()
    {
        _blackboard.AbilityReady = _abilitySystem.AbilityReady();
    }
}
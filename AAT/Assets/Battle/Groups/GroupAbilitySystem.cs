public class GroupAbilitySystem : GroupSystem, IAbilitySystem
{
    private bool _abilityReady;
    public bool AbilityReady() => _abilityReady;

    public void PrepareAbility(UnitAbilityDataInfo ability)
    {
        foreach (var member in group.GroupMembers)
        {
            member.GetComponent<IAbilitySystem>().PrepareAbility(ability);
        }
    }

    public void CastAbility(UnitAbilityDataInfo ability)
    {
        _abilityReady = true;
        
        foreach (var member in group.GroupMembers)
        {
            member.GetComponent<IAbilitySystem>().CastAbility(ability);
        }
    }
}
public interface IAbilitySystem
{
    public void PrepareAbility(UnitAbilityData ability);
    public void UnPrepareAbility(UnitAbilityData ability);
    public void CastAbility(UnitAbilityData ability, StumpTarget target);
}
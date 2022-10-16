public interface IAbilitySystem
{
    public bool AbilityReady();
    public void PrepareAbility(UnitAbilityDataInfo ability);
    public void CastAbility(UnitAbilityDataInfo ability);
}
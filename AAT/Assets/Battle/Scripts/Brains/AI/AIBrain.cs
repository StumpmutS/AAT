public class AIBrain : Brain
{
    protected override void Start()
    {
        base.Start();
        var stats = GetComponent<UnitStatsModifierManager>();
        GetComponent<TargetFinder>().Init(
            stats.GetStat(EUnitFloatStats.AttackRange), 
            stats.GetStat(EUnitFloatStats.SightRange));
    }
}

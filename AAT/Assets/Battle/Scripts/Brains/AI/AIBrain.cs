public class AIBrain : Brain
{
    protected override void Start()
    {
        base.Start();
        var stats = GetComponent<UnitStatsModifierManager>();
        GetComponent<TargetFinder>().Init(
            stats.CurrentStats[EUnitFloatStats.AttackRange], 
            stats.CurrentStats[EUnitFloatStats.SightRange]);
    }
}

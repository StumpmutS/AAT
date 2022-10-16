public class AIBrain : Brain<AiTransitionBlackboard>
{
    protected override AiTransitionBlackboard InitBlackboard() => new AiTransitionBlackboard();
}

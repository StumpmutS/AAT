public class EmptyAiComponentState : ComponentState<AiTransitionBlackboard>
{
    protected override void OnSpawnSuccess() { }

    protected override void OnEnter() { }

    protected override void Tick() { }

    public override void OnExit() { }
}
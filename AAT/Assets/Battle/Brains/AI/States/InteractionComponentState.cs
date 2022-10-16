using System;

public class InteractionComponentState : ComponentState<AiTransitionBlackboard>//TODO: consider different state class
{
    public event Action OnInteractionFinished = delegate { };

    public void FinishInteraction()
    {
        OnInteractionFinished.Invoke();
    }

    protected override void OnSpawnSuccess() { }

    protected override void OnEnter() { }

    protected override void Tick() { }

    public override void OnExit() { }
}

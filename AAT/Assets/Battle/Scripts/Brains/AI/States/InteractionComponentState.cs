using System;

public class InteractionComponentState : ComponentState//TODO: consider different state class
{
    public event Action OnInteractionFinished = delegate { };

    public void FinishInteraction()
    {
        OnInteractionFinished.Invoke();
    }

    public override void OnEnter() { }

    public override void OnExit() { }
}

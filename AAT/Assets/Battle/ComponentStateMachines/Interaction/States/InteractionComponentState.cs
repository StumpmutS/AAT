using System;
using UnityEngine;

public class InteractionComponentState : ComponentState<InteractionTransitionBlackboard>
{
    [SerializeField] private EInteractableType interactableType;
    public EInteractableType InteractableType => interactableType;
    
    public event Action<InteractionComponentState> OnInteractionFinished = delegate { };

    public void FinishInteraction()
    {
        OnInteractionFinished.Invoke(this);
    }

    protected override void OnSpawnSuccess() { }

    protected override void OnEnter() { }

    protected override void Tick() { }

    public override void OnExit() { }
}

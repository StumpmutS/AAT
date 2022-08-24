using System;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class InteractingController : SimulationBehaviour
{
    [SerializeField] private NetworkStateComponentContainer container;
    
    private InteractionComponentState _currentInteractionState;
    private bool _interacting;
    private bool _subscribed;
    
    public event Action OnInteractionFinished = delegate { };

    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsServer) return;

        if (_currentInteractionState == null) return;
        
        _currentInteractionState.Tick();
    }

    public void InteractWith(InteractableController interactable)
    {
        if (!Runner.IsServer || _interacting) return;
        _interacting = true;
        
        var initialState = interactable.RequestInteractionState();
        var interactionState = (InteractionComponentState) container.AddOrGetComponentState(initialState, null);
        
        _currentInteractionState = interactionState;
        SubscribeToCurrent();
        _currentInteractionState.TryOnEnter();
        interactable.RequestAffection(_currentInteractionState);
    }

    private void HandleInteractionComponentFinished()
    {
        if (!Runner.IsServer || !_interacting) return;
        _interacting = false;
        
        UnsubscribeFromCurrent();
        OnInteractionFinished.Invoke();
        _currentInteractionState.OnExit();
        _currentInteractionState = null;
    }

    private void SubscribeToCurrent()
    {
        if (!_subscribed)
        {
            _subscribed = true;
            _currentInteractionState.OnInteractionFinished += HandleInteractionComponentFinished;
        }
    }

    private void UnsubscribeFromCurrent()
    {
        if (_subscribed)
        {
            _subscribed = false;
            _currentInteractionState.OnInteractionFinished -= HandleInteractionComponentFinished;
        }
    }
}

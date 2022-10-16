using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class InteractingController : SimulationBehaviour
{
    [SerializeField] private NetworkComponentStateContainer<AiTransitionBlackboard> container;
    [SerializeField] private List<EInteractableType> interactableTypes;
    
    public List<EInteractableType> InteractableTypes => interactableTypes;
    
    private InteractionComponentState _currentInteractionState;
    private bool _interacting;
    private bool _subscribed;
    
    public event Action OnInteractionFinished = delegate { };

    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsServer) return;

        if (_currentInteractionState == null) return;
        
        _currentInteractionState.CallTick();
    }

    public void InteractWith(InteractableController interactable)
    {
        throw new NotImplementedException(); //TODO: Have interaction brain like the ability brain

        if (!Runner.IsServer || _interacting) return;
        _interacting = true;
        
        var initialState = interactable.RequestInteractionState();
        var interactionState = (InteractionComponentState) container.AddOrGetComponentState(initialState, null, null);
        
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

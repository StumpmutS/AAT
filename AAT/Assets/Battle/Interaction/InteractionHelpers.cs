using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

public static class InteractionHelpers
{
    public static bool InteractionReady(Transform transform, InteractableController interactable)
    {
        if (interactable == null) return false;
        if (!transform.TryGetComponent<Interactor>(out var interactor) || !interactor.InteractableTypes.Contains(interactable.InteractableType)) return false;
        return Vector3.Distance(transform.position, interactable.transform.position) <= interactable.InteractRange;
    }
    
    public static bool InteractionReady(IEnumerable<Transform> transforms, InteractableController interactable)
    {
        return transforms.All(t => InteractionReady(t, interactable));
    }

    public static void InitiateInteraction(InteractionBrain brain, InteractableController interactable, Action finishedCallback)
    {
        var interactionState = interactable.RequestInteractionState();
        if (brain.TrySetInteraction(interactionState, finishedCallback))
        {
            interactable.RequestAffection((InteractionComponentState) brain.AddOrGetState(interactionState));
        }
    }
}
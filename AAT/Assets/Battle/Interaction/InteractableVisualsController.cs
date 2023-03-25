using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class InteractableVisualsController : SimulationBehaviour
{
    [SerializeField] private InteractableController interactable;
    [SerializeField] private VisualComponentHandler visualHandler;
    [SerializeField] private VisualInfo visualInfo;
    [SerializeField] private List<VisualComponent> visuals;

    private void Awake()
    {
        SelectionManager.Instance.OnSelected += HandleSelected;
    }

    private void HandleSelected(IEnumerable<Selectable> selectables)
    {
        foreach (var selectable in selectables)
        {
            if (selectable.TryGetComponent<Interactor>(out var interactor) && interactor.InteractableTypes.Contains(interactable.InteractableType))
            {
                visualHandler.ActivateVisuals(visuals, visualInfo);
            }
        }
    }

    private void OnDestroy()
    {
        SelectionManager.Instance.OnSelected -= HandleSelected;
    }
}
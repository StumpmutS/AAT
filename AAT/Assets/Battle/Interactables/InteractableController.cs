using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class InteractableController : SimulationBehaviour, IDespawned //TODO: both sets up previews and handles interaction (bad)
{
    [SerializeField] private EInteractableType interactableType;
    [SerializeField] private float interactRange;
    public float InteractRange => interactRange;
    [SerializeField] private InteractionComponentState interactionComponentState;
    [SerializeField] private GameObject visualsPrefab;
    [SerializeField] private Vector3 visualsOffset;
    [SerializeField] protected UnitController unitController;
    [SerializeField] protected SelectableController selectable;

    private GameObject _visuals;
    protected PoolingObject _preview;
    private bool _previewDisplayed;

    public event Action<InteractableController> OnInteractableDestroyed = delegate { };

    protected virtual void Awake()
    {
        throw new NotImplementedException();
        unitController.OnDeath += DestroyInteractable;/*
        selectable.OnHover.AddListener(HandleHover);
        selectable.OnHoverStop.AddListener(HandleHoverStop);*/
    }

    private void DestroyInteractable(UnitController _)
    {
        OnInteractableDestroyed.Invoke(this);
    }
    
    private void Start()
    {
        var instantiatedVisuals = Instantiate(visualsPrefab, transform);
        instantiatedVisuals.transform.position += visualsOffset;
        instantiatedVisuals.gameObject.SetActive(false);
        _visuals = instantiatedVisuals;
        UnitManager.Instance.OnUnitSelected += HandleUnitSelected;
        UnitManager.Instance.OnUnitDeselected += HandleUnitDeselected;
    }

    private void DisplayVisuals()
    {
        _visuals.SetActive(true);
    }

    private void RemoveVisuals()
    {
        if (_visuals != null) _visuals.SetActive(false);
    }

    public abstract void RequestAffection(InteractionComponentState componentState);

    public InteractionComponentState RequestInteractionState() => interactionComponentState;

    public virtual InteractableController DetermineInteractable(UnitController _) => this;

    public void TryDisplayPreview(UnitController unit)
    {
        throw new NotImplementedException();
        /*if (_previewDisplayed || !unit.InteractableTypes.Contains(interactableType) || TeamRelations.TeamRelation(unit.Team, unitController.Team, ETeamRelation.Enemy)) return;
        _previewDisplayed = true;
        DisplayPreview(unit);*/
    }

    protected virtual void DisplayPreview(UnitController unit)
    {
        _preview = PoolingManager.Instance.CreatePoolingObject(unit.UnitVisuals);
        _preview.transform.position = transform.position;
    }

    public void TryRemovePreview()
    {
        if (!_previewDisplayed) return;
        _previewDisplayed = false;
        RemovePreview();
    }

    protected virtual void RemovePreview()
    {
        _preview.Deactivate();
    }

    private void HandleUnitSelected(UnitController unit)
    {
        throw new NotImplementedException();
        /*if (unit.InteractableTypes.Contains(interactableType) && !TeamRelations.TeamRelation(unit.Team, unitController.Team, ETeamRelation.Enemy)) DisplayVisuals();
        else RemoveVisuals();*/
    }

    private void HandleUnitDeselected(UnitController unit)
    {
        RemoveVisuals();
    }

    private void HandleHover()
    {
        foreach (var selectedUnit in UnitManager.Instance.SelectedUnits)
        {
            TryDisplayPreview(selectedUnit);
            return;
        }
    }

    private void HandleHoverStop() => TryRemovePreview();
    public void Despawned(NetworkRunner runner, bool hasState)
    {
        UnitManager.Instance.OnUnitSelected -= HandleUnitSelected;
        UnitManager.Instance.OnUnitDeselected -= HandleUnitDeselected;
    }

    private void OnDestroy()
    {
        throw new NotImplementedException();
        if (unitController != null) unitController.OnDeath -= DestroyInteractable;/*
        if (selectable != null) selectable.OnHover.RemoveListener(HandleHover);
        if (selectable != null) selectable.OnHoverStop.RemoveListener(HandleHoverStop);*/
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableController : MonoBehaviour
{
    [SerializeField] private EInteractableType interactableType;
    public EInteractableType InteractableType => interactableType;
    [SerializeField] private GameObject visualsPrefab;
    [SerializeField] private Vector3 visualsOffset;
    [SerializeField] protected SelectableController selectable;

    private GameObject _visuals;
    protected PoolingObject _preview;
    private bool _previewDisplayed;

    public event Action<InteractableController> OnInteractableDestroyed = delegate {  };

    protected virtual void Awake()
    {
        InteractableManager.Instance.AddInteractable(this);
        selectable.OnHover += HoverHandler;
        selectable.OnHoverStop += HoverStopHandler;
        if (selectable is UnitController unit) unit.OnDeath += DestroyInteractable;
    }

    private void DestroyInteractable(UnitController notNeeded)
    {
        OnInteractableDestroyed.Invoke(this);
    }
    
    private void Start()
    {
        var instantiatedVisuals = Instantiate(visualsPrefab, transform);
        instantiatedVisuals.transform.position += visualsOffset;
        instantiatedVisuals.gameObject.SetActive(false);
        _visuals = instantiatedVisuals;
    }

    public void DisplayVisuals()
    {
        _visuals.SetActive(true);
    }

    public void RemoveVisuals()
    {
        if (_visuals != null) _visuals.SetActive(false);
    }

    private void HoverHandler()
    {
        InteractableManager.Instance.SetHoveredInteractable(this);
    }

    private void HoverStopHandler()
    {
        InteractableManager.Instance.RemoveHoveredInteractable(this);
    }

    public virtual void SetupInteractions(List<UnitController> units)
    {
        foreach (var unit in units)
        {
            unit.Interact(this, RequestAffection);
        }
    }

    protected abstract void RequestAffection(UnitController unit);

    public void CallDisplayPreview(PoolingObject previewObject, int unitAmount)
    {
        if (_previewDisplayed) return;
        _previewDisplayed = true;
        DisplayPreview(previewObject, unitAmount);
    }

    protected virtual void DisplayPreview(PoolingObject previewObject, int unitAmount)
    {
        _preview = PoolingManager.Instance.CreatePoolingObject(previewObject);
    }

    public void RemovePreview()
    {
        if (!_previewDisplayed) return;
        _previewDisplayed = false;
        _preview.Deactivate();
    }
}

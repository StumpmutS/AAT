using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectableController), typeof(Collider))]
public abstract class BaseMountableController : InteractableController //come up with better inheritance (ticket)
{
    [SerializeField] protected MountablePointLinkController _mountablePointLink;

    public event Action<BaseMountableController> OnMountableHover = delegate { };
    public event Action OnMountableHoverStop = delegate { };

    private bool _previewDisplayed;
    private PoolingObject _preview;

    public abstract BaseMountableDataInfo ReturnData();

    protected override void Start()
    {
        base.Start();
        MountManager.Instance.AddMountable(this);
    }

    public void ResetLink(MountablePointLinkController newLink)
    {
        _mountablePointLink = newLink;
    }

    protected override void HoverHandler()
    {
        base.HoverHandler();
        OnMountableHover.Invoke(this);
    }

    protected override void HoverStopHandler()
    {
        base.HoverStopHandler();
        OnMountableHoverStop.Invoke();
    }

    public void CallDisplayPreview(PoolingObject transportableUnitPreview, int unitAmount)
    {
        if (_previewDisplayed) return;
        _previewDisplayed = true;
        DisplayPreview(transportableUnitPreview, unitAmount);
    }

    protected virtual void DisplayPreview(PoolingObject transportableUnitPreview, int unitAmount)
    {
        _preview = PoolingManager.Instance.CreatePoolingObject(transportableUnitPreview);
        Transform previewTransform = _preview.transform;
        previewTransform.position = transform.position;
        previewTransform.rotation = transform.rotation;
        previewTransform.SetParent(transform);
        MountManager.Instance.AddHoveredMountable(this);

        if (unitAmount > 1)
        {
            _mountablePointLink.BeginPreviewDisplayLink(this, transportableUnitPreview, unitAmount - 1, true); //CHANGE TRUE TO BE BASED OFF MOUSE POSITION IN RELATION TO COLLIDER
        }
    }

    public void RemovePreview()
    {
        if (!_previewDisplayed) return;
        _previewDisplayed = false;
        _preview.Deactivate();
    }

    public virtual void ActivateMounted(ArmoredHealthUnitStatsData stats) { }
    public virtual void DeactivateMounted(ArmoredHealthUnitStatsData stats) { }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectableController), typeof(Collider))]
public abstract class BaseMountableController : MonoBehaviour
{
    [SerializeField] protected MountablePointLinkController _mountablePointLink;
    [SerializeField] private GameObject mountableVisualsPrefab;
    [SerializeField] private Vector3 visualsOffset;

    public event Action<BaseMountableController> OnMountableHover = delegate { };
    public event Action OnMountableHoverStop = delegate { };

    private bool _visualsDisplayed;
    private bool _previewDisplayed;
    private SelectableController _selectableController;
    private PoolingObject _preview;

    public abstract BaseMountableDataInfo ReturnData();

    protected virtual void Awake()
    {
        _selectableController = GetComponent<SelectableController>();
        _selectableController.OnHover += Hover;
        _selectableController.OnHoverStop += StopHover;
    }

    private void Start()
    {
        MountManager.Instance.AddMountable(this);
        mountableVisualsPrefab = Instantiate(mountableVisualsPrefab, gameObject.transform);
        mountableVisualsPrefab.transform.position += visualsOffset;
        mountableVisualsPrefab.SetActive(false);
    }

    public void ResetLink(MountablePointLinkController newLink)
    {
        _mountablePointLink = newLink;
    }

    private void Hover()
    {
        OnMountableHover.Invoke(this);
    }

    private void StopHover()
    {
        OnMountableHoverStop.Invoke();
    }

    public void DisplayVisuals()
    {
        if (_visualsDisplayed) return;
        _visualsDisplayed = true;
        mountableVisualsPrefab.SetActive(true);
    }

    public void RemoveVisuals()
    {
        if (!_visualsDisplayed) return;
        _visualsDisplayed = false;
        mountableVisualsPrefab.SetActive(false);
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

    public virtual void ActivateMounted(UnitStatsDataInfo stats) { }
    public virtual void DeactivateMounted(UnitStatsDataInfo stats) { }
}

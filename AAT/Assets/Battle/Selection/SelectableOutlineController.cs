using System;
using UnityEngine;

public class SelectableOutlineController : MonoBehaviour
{
    [SerializeField] private SelectableController selectable;
    [SerializeField] private Hoverable hoverable;
    [SerializeField] private OutlineController outline;

    public event Action OnOutline = delegate { };
    public event Action OnRemoveOutline = delegate { };

    private bool _outlined;

    private void Awake()
    {
        selectable.OnSelect.AddListener(HandleSelect);
        selectable.OnDeselect.AddListener(HandleDeselect);
        hoverable.OnHover.AddListener(HandleMouseEnter);
        hoverable.OnHoverStop.AddListener(HandleMouseExit);
    }

    private void HandleMouseEnter()
    {
        Outline();
    }

    private void HandleMouseExit()
    {
        RemoveOutline();
    }

    private void HandleSelect()
    {
        Outline();
    }

    private void HandleDeselect()
    {
        RemoveOutline();
    }
    
    public void Outline()
    {
        if (_outlined) return;
        _outlined = true;
        outline.Activate();
        OnOutline.Invoke();
    }
    
    public void RemoveOutline()
    {
        if (selectable.Selected || !_outlined) return;

        _outlined = false;
        outline.Deactivate();
        OnRemoveOutline.Invoke();
    }

    private void OnDestroy()
    {
        if (selectable != null) selectable.OnSelect.RemoveListener(HandleSelect);
        if (selectable != null) selectable.OnDeselect.RemoveListener(HandleDeselect);
        if (hoverable != null) hoverable.OnHover.RemoveListener(HandleMouseEnter);
        if (hoverable != null) hoverable.OnHoverStop.RemoveListener(HandleMouseExit);
    }
}

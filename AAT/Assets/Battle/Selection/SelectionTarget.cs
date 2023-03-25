using System;
using UnityEngine;

public class SelectionTarget : MonoBehaviour
{
    [SerializeField] private Selectable selectable;
    [SerializeField] private Hoverable hoverable;

    public Selectable Selectable
    {
        get => _selectable;
        set
        {
            if (_selectable != null) _selectable.OnSelect.RemoveListener(HandleSelect);
            if (_selectable != null) _selectable.OnDeselect.RemoveListener(HandleDeselect);
            _selectable = value;
            value.OnSelect.AddListener(HandleSelect);
            value.OnDeselect.AddListener(HandleDeselect);
        }
    }

    public Hoverable Hoverable
    {
        get => _hoverable;
        set
        {
            if (_hoverable != null) _hoverable.OnHover.RemoveListener(HandleHover);
            if (_hoverable != null) _hoverable.OnHoverStop.RemoveListener(HandleHoverStop);
            _hoverable = value;
            value.OnHover.AddListener(HandleHover);
            value.OnHoverStop.AddListener(HandleHoverStop);
        }
    }

    private Selectable _selectable;
    private Hoverable _hoverable;
    
    public event Action<Selectable> OnSelect = delegate { };
    public event Action<Selectable> OnDeselect = delegate { };
    public event Action<Hoverable> OnHover = delegate { };
    public event Action<Hoverable> OnHoverStop = delegate { };

    private void Awake()
    {
        if (selectable != null) SetSelectable(selectable);
        if (hoverable != null) SetHoverable(hoverable);
    }

    private void Start()
    {
        SelectionManager.Instance.AddSelectionTarget(this);
    }

    public void SetSelectable(Selectable selectable)
    {
        Selectable = selectable;
    }

    public void SetHoverable(Hoverable hoverable)
    {
        Hoverable = hoverable;
    }

    private void HandleSelect() => OnSelect.Invoke(Selectable);
    private void HandleDeselect() => OnDeselect.Invoke(Selectable);
    private void HandleHover() => OnHover.Invoke(Hoverable);
    private void HandleHoverStop() => OnHoverStop.Invoke(Hoverable);

    private void OnDestroy()
    {
        SelectionManager.Instance.RemoveSelectionTarget(this);
        if (_selectable != null) _selectable.OnSelect.RemoveListener(HandleSelect);
        if (_selectable != null) _selectable.OnDeselect.RemoveListener(HandleDeselect);
        if (_hoverable != null) _hoverable.OnHover.RemoveListener(HandleHover);
        if (_hoverable != null) _hoverable.OnHoverStop.RemoveListener(HandleHoverStop);
    }
}
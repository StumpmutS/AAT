using System;
using UnityEngine;

[RequireComponent(typeof(Selectable), typeof(Hoverable))]
public class SelectionSpring : MonoBehaviour
{
    [SerializeField] private SpringController springs;
    [SerializeField] private float nudgeSpeed;
    
    private Selectable _selectable;
    private Hoverable _hoverable;

    private void Awake()
    {
        _selectable = GetComponent<Selectable>();
        BaseInputManager.OnLeftCLickUp += HandleClick;
        _selectable.OnDeselect.AddListener(HandleDeselect);
        _hoverable = GetComponent<Hoverable>();
        _hoverable.OnHover.AddListener(HandleHover);
        _hoverable.OnHoverStop.AddListener(HandleHoverStop);
    }

    private void HandleClick()
    {
        if (_selectable.MouseOver) springs.Nudge(nudgeSpeed);
    }

    private void HandleDeselect()
    {
        springs.SetTarget(0);
    }

    private void HandleHover()
    {
        springs.SetTarget(1);
    }

    private void HandleHoverStop()
    {
        if (_selectable.Selected) return;
        springs.SetTarget(0);
    }

    private void OnDestroy()
    {
        BaseInputManager.OnLeftCLickUp -= HandleClick;
        if (_selectable != null) _selectable.OnDeselect.AddListener(HandleDeselect);
        if (_selectable != null) _hoverable.OnHover.AddListener(HandleHover);
        if (_selectable != null) _hoverable.OnHoverStop.AddListener(HandleHoverStop);
    }
}

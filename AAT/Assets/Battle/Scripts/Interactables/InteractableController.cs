using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectableController))]
public class InteractableController : MonoBehaviour
{
    [SerializeField] private GameObject visualsPrefab;
    [SerializeField] private Vector3 visualsOffset;

    public UnitController Unit { get; private set; }

    private GameObject _visuals;

    protected virtual void Awake()
    {
        InteractableManager.Instance.AddInteractable(this);
        Unit = GetComponent<UnitController>();
        Unit.OnHover += HoverHandler;
        Unit.OnHoverStop += HoverStopHandler;
    }

    protected virtual void Start()
    {
        var instantiateVisuals = Instantiate(visualsPrefab, transform);
        instantiateVisuals.transform.position += visualsOffset;
        instantiateVisuals.gameObject.SetActive(false);
        _visuals = instantiateVisuals;
    }

    public void DisplayVisuals()
    {
        _visuals.SetActive(true);
    }

    public void RemoveVisuals()
    {
        _visuals.SetActive(false);
    }

    protected virtual void HoverHandler()
    {
        InteractableManager.Instance.SetHoveredInteractable(this);
    }

    protected virtual void HoverStopHandler()
    {
        InteractableManager.Instance.RemoveHoveredInteractable(this);
    }

    public virtual void Affect(UnitController unit)
    {

    }
}

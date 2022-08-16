using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseMountableController : InteractableController
{
    [SerializeField] private SelfOtherStatsData mountData;
    public SelfOtherStatsData MountData => mountData;
    [SerializeField] private MountablePointLinkController mountablePointLink;
    public UnitController Unit => unitController;
    
    private InteractionComponentState _mounted;
    private int _previewIndex;
    
    public void SetLink(MountablePointLinkController newLink)
    {
        mountablePointLink = newLink;
    }

    public override void RequestAffection(InteractionComponentState componentState)
    {
        ((TransportedComponentState) componentState).Mount(this);
        _mounted = componentState;
    }

    public override InteractableController DetermineInteractable(UnitController unit)
    {
        var mountables = mountablePointLink.DetermineMountables(this, unit, true);
        //return mountables[unit.SelectedIndex]; todo
        return null;
    }

    protected override void DisplayPreview(UnitController unit)
    {
        base.DisplayPreview(unit);
        var previewTransform = _preview.transform;
        previewTransform.position = transform.position;
        previewTransform.rotation = transform.rotation;
        previewTransform.SetParent(transform);

        if (unit.UnitGroup.Units.Count > 1)
        {
            mountablePointLink.BeginPreviewDisplayLink(this, unit, true);
        }
    }

    protected override void RemovePreview()
    {
        mountablePointLink.RemovePreviewDisplay();
    }

    public void ActivateMounted(BaseUnitStatsData stats)
    {
        if (unitController == null) return;
        unitController.ModifyStats(stats);
        unitController.ModifyStats(mountData.SelfModifier);
    }

    public void DeactivateMounted(BaseUnitStatsData stats) //TODO: will never be called because interaction states dont know when exit
    {
        if (unitController == null) return;
        unitController.ModifyStats(stats, false);
        unitController.ModifyStats(mountData.SelfModifier, false);
        _mounted.FinishInteraction();
        _mounted = null;
    }
}

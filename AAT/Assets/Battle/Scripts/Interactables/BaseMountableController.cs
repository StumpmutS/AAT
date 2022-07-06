using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseMountableController : InteractableController
{
    [SerializeField] private SelfOtherStatsData mountData;
    public SelfOtherStatsData MountData => mountData;
    [SerializeField] private MountablePointLinkController mountablePointLink;
    [SerializeField] private UnitController unit;
    public UnitController Unit => unit;
    
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
        //TODO: CHANGE TRUE TO BE BASED OFF MOUSE POSITION IN RELATION TO COLLIDER
        var mountables = mountablePointLink.DetermineMountables(this, unit.UnitGroup.Units.Count, true);
        return mountables[unit.GroupIndex];
    }

    protected override void DisplayPreview(PoolingObject transportableUnitPreview, int unitAmount)
    {
        base.DisplayPreview(transportableUnitPreview, unitAmount);
        var previewTransform = _preview.transform;
        previewTransform.position = transform.position;
        previewTransform.rotation = transform.rotation;
        previewTransform.SetParent(transform);

        if (unitAmount > 1)
        {
            //TODO: CHANGE TRUE TO BE BASED OFF MOUSE POSITION IN RELATION TO COLLIDER
            mountablePointLink.BeginPreviewDisplayLink(this, transportableUnitPreview, unitAmount - 1, true);
        }
    }

    public void ActivateMounted(BaseUnitStatsData stats)
    {
        if (unit == null) return;
        unit.ModifyStats(stats);
        unit.ModifyStats(mountData.SelfModifier);
    }

    public void DeactivateMounted(BaseUnitStatsData stats) //TODO: will never be called because interaction states dont know when exit
    {
        if (unit == null) return;
        unit.ModifyStats(stats, false);
        unit.ModifyStats(mountData.SelfModifier, false);
        _mounted.FinishInteraction();
        _mounted = null;
    }
}

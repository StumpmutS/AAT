using System;
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

    public override void SetupInteraction(MovementInteractOverrideComponentState componentState)
    {
        var previewedMountable = mountablePointLink.PreviewedMountables[_previewIndex];
        componentState.Interact(previewedMountable, previewedMountable.RequestAffection);
        _previewIndex++;
        if (_previewIndex > mountablePointLink.PreviewedMountables.Count - 1) _previewIndex = 0;//TODO: assumes only calling from manager that would have already displayed previews
    }

    protected override void RequestAffection(InteractionComponentState componentState)
    {
        ((TransportedComponentState) componentState).Mount(this);
        _mounted = componentState;
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
            mountablePointLink.BeginPreviewDisplayLink(this, transportableUnitPreview, unitAmount - 1, true); //TODO: CHANGE TRUE TO BE BASED OFF MOUSE POSITION IN RELATION TO COLLIDER
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

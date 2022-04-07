using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class BaseMountableController : InteractableController
{
    [SerializeField] private SelfOtherStatsData mountData;
    public SelfOtherStatsData MountData => mountData;
    [SerializeField] private MountablePointLinkController mountablePointLink;
    [SerializeField] private UnitController unit;
    public UnitController Unit => unit;

    public void SetLink(MountablePointLinkController newLink)
    {
        mountablePointLink = newLink;
    }

    public override void SetupInteractions(List<UnitController> units)
    {
        var previewedMountables = mountablePointLink.PreviewedMountables;
        for (int i = 0; i < previewedMountables.Count; i++)
        {
            units[i].Interact(previewedMountables[i], previewedMountables[i].RequestAffection);
        }
    }

    protected override void RequestAffection(UnitController unit)
    {
        unit.FinishInteraction();
        unit.GetComponent<TransportableController>().Mount(this);
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

    public virtual void DeactivateMounted(BaseUnitStatsData stats)
    {
        if (unit == null) return;
        unit.ModifyStats(stats, false);
        unit.ModifyStats(mountData.SelfModifier, false);
    }
}

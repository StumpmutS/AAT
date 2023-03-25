using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseMountableController : InteractableController
{
    [SerializeField] private SelfOtherStatsData mountData;
    public SelfOtherStatsData MountData => mountData;
    [SerializeField] private MountablePointLinkController mountablePointLink;
    
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

    /*public override InteractableController DetermineInteractable(Group group)
    {/*
        var mountables = mountablePointLink.DetermineMountables(this, unit, true);#1#
        //return mountables[unit.SelectedIndex]; todo
        return null;
    }*//*

    protected override void DisplayPreview()
    {/*
        base.DisplayPreview(unit);#1#
        var previewTransform = _preview.transform;
        previewTransform.position = transform.position;
        previewTransform.rotation = transform.rotation;
        previewTransform.SetParent(transform);

        throw new NotImplementedException();
        /*if (unit.UnitGroup.Units.Count > 1)
        {
            mountablePointLink.BeginPreviewDisplayLink(this, unit, true);
        }#1#
    }

    protected override void RemovePreview()
    {
        mountablePointLink.RemovePreviewDisplay();
    }*/

    public void ActivateMounted(BaseUnitStatsData stats)
    {/*
        if (SelectableController == null) return;
        SelectableController.ModifyStats(stats);*/
        //unitController.ModifyStats(mountData.SelfModifier);
    }

    public void DeactivateMounted(BaseUnitStatsData stats) //TODO: will never be called because interaction states dont know when exit
    {/*
        if (SelectableController == null) return;
        SelectableController.ModifyStats(stats, false);*/
        //unitController.ModifyStats(mountData.SelfModifier, false);
        _mounted.FinishInteraction();
        _mounted = null;
    }
}

using System.Collections;
using UnityEngine;

public class TeleportPoint : InteractableController
{
    public UnitController Unit => unitController;
    [SerializeField] private Vector3 exitPointOffset;
    [SerializeField] private float teleportTime;
    public float TeleportTime => teleportTime;

    private SectorController _sector;
    public TeleportPoint OtherTeleportPoint { get; private set; }
    
    private bool _hoverSubscribed;

    protected override void Awake()
    {
        base.Awake();
        selectable.OnSelect.AddListener(SelectTeleporter);
        selectable.OnDeselect.RemoveListener(DeselectTeleporter);
    }

    public void SetupPair(SectorController sector, TeleportPoint other, SectorController otherSector)
    {
        _sector = sector;
        OtherTeleportPoint = other;
        other._sector = otherSector;
        other.OtherTeleportPoint = this;
        SectorManager.Instance.AddTeleportPointPair(this, other, sector, otherSector);
    }

    private void SelectTeleporter()
    {
        OtherTeleportPoint.Unit.OutlineSelectable.CallSelect();
    }

    private void DeselectTeleporter()
    {
        OtherTeleportPoint.Unit.OutlineSelectable.CallDeselect();
    }

    public override void RequestAffection(InteractionComponentState componentState)
    {
        StartCoroutine(WarpInteractor(componentState));
    }

    private IEnumerator WarpInteractor(InteractionComponentState componentState)
    {
        yield return new WaitForSeconds(teleportTime);
        if (componentState == null) yield break;
        
        componentState.Container.GetComponent<AgentBrain>().CurrentAgent.Warp(OtherTeleportPoint.transform.position + OtherTeleportPoint.exitPointOffset);
        componentState.Container.GetComponent<UnitController>().SetSector(OtherTeleportPoint._sector);
        componentState.FinishInteraction();
    }

    protected override void DisplayPreview(UnitController unit)
    {
        base.DisplayPreview(unit);
        _preview.transform.position = OtherTeleportPoint.transform.position;
    }
}

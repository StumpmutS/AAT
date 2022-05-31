using System.Collections;
using UnityEngine;

public class TeleportPoint : InteractableController
{
    [SerializeField] private Vector3 exitPointOffset;
    [SerializeField] private float teleportTime;
    public float TeleportTime => teleportTime;

    public UnitController Unit { get; private set; }

    private SectorController _sector;
    public TeleportPoint OtherTeleportPoint { get; private set; }
    
    private bool _hoverSubscribed;

    protected override void Awake()
    {
        base.Awake();
        selectable.OnSelect += SelectTeleporter;
        selectable.OnDeselect += DeselectTeleporter;
        Unit = selectable as UnitController;
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
        OtherTeleportPoint.Unit.CallSelect();
    }

    private void DeselectTeleporter()
    {
        OtherTeleportPoint.Unit.CallDeselect();
    }

    protected override void RequestAffection(InteractionComponentState componentState)
    {
        StartCoroutine(WarpInteractor(componentState));
    }

    private IEnumerator WarpInteractor(InteractionComponentState componentState)
    {
        yield return new WaitForSeconds(teleportTime);
        componentState.GetComponent<AgentBrain>().CurrentAgent.Warp(OtherTeleportPoint.transform.position + OtherTeleportPoint.exitPointOffset);
        componentState.GetComponent<UnitController>().SetSector(OtherTeleportPoint._sector);
        componentState.FinishInteraction();
    }
}

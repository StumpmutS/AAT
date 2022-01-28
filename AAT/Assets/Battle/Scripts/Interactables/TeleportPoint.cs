using UnityEngine;

public class TeleportPoint : InteractableController
{
    [SerializeField] private Vector3 exitPointOffset;

    public UnitController Unit { get; private set; }

    private SectorController _sector;
    private TeleportPoint _otherTeleportPoint;
    private bool _hoverSubscribed;

    protected override void Awake()
    {
        base.Awake();
        selectable.OnSelect += SelectTeleporter;
        selectable.OnDeselect += DeselectTeleporter;
        Unit = selectable as UnitController;
    }

    public void Setup(SectorController sector, TeleportPoint other)
    {
        _sector = sector;
        _otherTeleportPoint = other;
    }

    private void SelectTeleporter()
    {
        _otherTeleportPoint.Unit.CallSelect();
    }

    private void DeselectTeleporter()
    {
        _otherTeleportPoint.Unit.CallDeselect();
    }

    protected override void RequestAffection(UnitController unit)
    {
        unit.GetComponent<AATAgentController>().Warp(_otherTeleportPoint.transform.position + _otherTeleportPoint.exitPointOffset);
        unit.SetSector(_sector);
    }
}

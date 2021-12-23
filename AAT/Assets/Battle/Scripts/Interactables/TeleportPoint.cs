using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : InteractableController
{
    [SerializeField] private Vector3 exitPointOffset;
    public Vector3 ExitPointOffset => exitPointOffset;

    public SectorController _sector { get; private set; }
    private TeleportPoint _otherTeleportPoint;
    private bool _hoverSubscribed;

    protected override void Awake()
    {
        base.Awake();
        Unit.OnSelect += SelectTeleporter;
        Unit.OnDeselect += DeselectTeleporter;
    }

    public void Setup(SectorController sector, TeleportPoint other)
    {
        _sector = sector;
        _otherTeleportPoint = other;
    }

    private void SelectTeleporter()
    {
        _otherTeleportPoint.Unit.Select();
    }

    private void DeselectTeleporter()
    {
        _otherTeleportPoint.Unit.Deselect();
    }

    public override void Affect(UnitController unit)
    {
        unit.GetComponent<AATAgentController>().Warp(_otherTeleportPoint.transform.position + _otherTeleportPoint.exitPointOffset);
    }
}

using System;
using UnityEngine;

[RequireComponent(typeof(SectorReference))]
public class SectorUpdater : MonoBehaviour
{
    private SectorReference _sectorReference;
    private SectorController _currentSector;

    private void Awake()
    {
        _sectorReference = GetComponent<SectorReference>();
        _sectorReference.OnSectorChanged += HandleSectorChanged;
    }

    private void HandleSectorChanged()
    {
        if (_currentSector != null) _currentSector.RemoveMember(this);
        
        _currentSector = _sectorReference.Sector;
        _currentSector.AddMember(this);
    }
}
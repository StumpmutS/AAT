using System;
using Fusion;
using UnityEngine;

public class SectorReference : SimulationBehaviour, ISpawned
{
    [SerializeField] private SectorController startSector;
    [SerializeField] protected float searchRadius = .5f;

    private SectorController _sector;
    public SectorController Sector
    {
        get => _sector;
        protected set
        {
            if (value == null || _sector == value) return;
            
            _sector = value;
            OnSectorChanged.Invoke();
        }
    }

    public event Action OnSectorChanged = delegate { };

    public virtual void Spawned()
    {
        _sector = startSector;

        if (_sector == null)
        {
            _sector = FindSector();
        }
    }

    public void RefreshSector()
    {
        _sector = FindSector();
    }

    protected SectorController FindSector()
    {
        var pos0Y = transform.position;
        pos0Y.y = 0;
        return SectorFinder.FindSector(pos0Y, searchRadius, LayerManager.Instance.GroundLayer);
    }
}
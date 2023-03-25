using System;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(Group))]
public class TeleportPairer : SimulationBehaviour //TODO:
{
    private Group _group;
    private TeleportPoint _lastFirst;
    private TeleportPoint _lastSecond;
    
    private void Awake()
    {
        _group = GetComponent<Group>();
        _group.OnMembersChanged += SetTeleporters;
    }

    private void SetTeleporters()
    {
        if (_group.GroupMembers.Count < 2) return;
        if (_group.GroupMembers.Count > 2)
        {
            Debug.LogError("Only pairs of teleporters are supported currently, no more than two teleporters to a teleporter group");
            throw new NotImplementedException();
        }

        TeleportPoint firstTeleportPoint = null;
        foreach (var member in _group.GroupMembers)
        {
            if (firstTeleportPoint == null)
            {
                firstTeleportPoint = member.GetComponent<TeleportPoint>();
                continue;
            }

            var secondTeleportPoint = member.GetComponent<TeleportPoint>();
            if (firstTeleportPoint == _lastFirst && secondTeleportPoint == _lastSecond) return;
            
            firstTeleportPoint.SetupPair(secondTeleportPoint);

            var fromSector = SectorFinder.FindSector(firstTeleportPoint.transform.position, .5f, LayerManager.Instance.GroundLayer);
            var toSector = SectorFinder.FindSector(secondTeleportPoint.transform.position, .5f, LayerManager.Instance.GroundLayer);
            SectorManager.Instance.AddTeleportPointPair(firstTeleportPoint, secondTeleportPoint, fromSector, toSector);

            _lastFirst = firstTeleportPoint;
            _lastSecond = secondTeleportPoint;
        }
    }

    private void OnDestroy()
    {
        if (_group != null) _group.OnMembersChanged -= SetTeleporters;
    }
}
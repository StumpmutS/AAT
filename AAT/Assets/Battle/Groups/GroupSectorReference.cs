using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

[RequireComponent(typeof(Group))]
public class GroupSectorReference : SectorReference
{
    private Group _group;
    private HashSet<SectorReference> _memberSectorReferences = new();

    public override void Spawned()
    {
        base.Spawned();
        _group = GetComponent<Group>();
        _group.OnMembersChanged += RefreshMemberSectorReferences;
    }

    private void RefreshMemberSectorReferences()
    {
        _memberSectorReferences.Clear();

        foreach (var member in _group.GroupMembers)
        {
            if (member.TryGetComponent<SectorReference>(out var sectorReference))
            {
                _memberSectorReferences.Add(sectorReference);
                sectorReference.OnSectorChanged += RefreshSector;
            }
        }
        
        RefreshSector();
    }

    private void RefreshSector()
    {
        var membersPerSector = new Dictionary<SectorController, int>();
        foreach (var sectorReference in _memberSectorReferences)
        {
            membersPerSector.TryGetValue(sectorReference.Sector, out var count);
            membersPerSector[sectorReference.Sector] = count + 1;
        }
        
        var highest = membersPerSector.MaxKeyByValue();
        if (highest == null) return;
        if (highest != Sector)
        {
            foreach (var member in _group.GroupMembers)
            {
                Sector.RemoveMember(member);
            }
        }
        
        Sector = highest;
        
        foreach (var member in _group.GroupMembers)
        {
            highest.AddMember(member);
        }
    }
}
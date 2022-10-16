using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Group : NetworkBehaviour
{
    [Tooltip("No duplicates")]
    [SerializeField] private List<GroupMember> groupMembers;

    public HashSet<GroupMember> GroupMembers { get; private set; }
    public SectorController Sector { get; private set; }

    private void Awake()
    {
        GroupMembers = new HashSet<GroupMember>(groupMembers);
    }

    public void Init(SectorController sector)
    {
        Sector = sector;
    }
    
    public void AddMember(GroupMember member)
    {
        GroupMembers.Add(member);
    }
}
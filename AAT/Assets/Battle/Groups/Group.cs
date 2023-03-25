using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Group : NetworkBehaviour
{
    [Tooltip("No duplicates")]
    [SerializeField] private List<GroupMember> groupMembers;
    [SerializeField] private SectorReference sectorReference;

    public HashSet<GroupMember> GroupMembers { get; private set; }
    public SectorController Sector => sectorReference.Sector;
    
    public event Action OnMembersChanged = delegate { }; 

    private void Awake()
    {
        GroupMembers = new HashSet<GroupMember>(groupMembers);
    }
    
    public void AddMember(GroupMember member)
    {
        GroupMembers.Add(member);
        OnMembersChanged.Invoke();
    }

    public List<TransformChain> GetCallingPoints()
    {
        List<TransformChain> chains = new();

        foreach (var member in GroupMembers)
        {
            if (member.TryGetComponent<TransformContainer>(out var container))
            {
                chains.Add(container.ToChain());
                continue;
            }
            
            chains.Add(new TransformChain(new [] {member.transform}));
        }

        return chains;
    }
}
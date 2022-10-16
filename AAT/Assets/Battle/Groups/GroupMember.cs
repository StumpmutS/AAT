using System;
using Fusion;
using UnityEngine;

public class GroupMember : SimulationBehaviour
{
    [SerializeField] private float radius;
    
    public TeamController Team { get; private set; }
    public SectorController Sector { get; private set; }
    public Group Group { get; private set; }

    private void Awake()
    {
        throw new NotImplementedException();
    }

    public void Init(int teamNumber, SectorController sector, Group group)
    {
        Team.SetTeamNumber(teamNumber);
        Sector = sector;
        Group = group;
    }
}
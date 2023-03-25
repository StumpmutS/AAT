using System;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(TeamController))]
public class GroupMember : SimulationBehaviour
{
    [SerializeField] private float radius;
    
    public TeamController Team { get; private set; }
    public Group Group { get; private set; }

    private void Awake()
    {
        Team = GetComponent<TeamController>();
    }

    public void Init(int teamNumber, Group group)
    {
        Team.SetTeamNumber(teamNumber);
        Group = group;
    }
}
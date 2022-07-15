using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private int maxTeams;
    
    public static TeamManager Instance { get; private set; }

    private List<int> _teamNumbers = new();

    private void Awake()
    {
        Instance = this;
    }

    public LayerMask GetEnemyLayer(int teamNumber)
    {
        HashSet<string> layerNames = new();

        foreach (var number in _teamNumbers)
        {
            if (number == teamNumber) continue;
            layerNames.Add($"Team{number}");
        }
        
        return LayerMask.GetMask(layerNames.ToArray());
    }

    public void SetupWithTeam(TeamController teamController, int desiredTeamNumber = 0)
    {
        if (_teamNumbers.Contains(desiredTeamNumber))
        {
            teamController.SetTeamNumber(desiredTeamNumber);
            return;
        }

        for (int i = 1; i <= maxTeams; i++)
        {
            if (_teamNumbers.Contains(i)) continue;
            
            _teamNumbers.Add(i);
            teamController.SetTeamNumber(i);
            return;
        }
        
        Debug.LogError("No team numbers available");
    }
}

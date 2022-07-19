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

    [Networked, Capacity(16)] private NetworkArray<NetworkBool> _teamNumbers => default;

    private void Awake()
    {
        Instance = this;
    }

    public LayerMask GetEnemyLayer(int teamNumber)
    {
        HashSet<string> layerNames = new();

        for (int i = 0; i < _teamNumbers.Length; i++)
        {
            if (i + 1 == teamNumber || !_teamNumbers[i]) continue;
            layerNames.Add($"Team{i + 1}");
        }

        return LayerMask.GetMask(layerNames.ToArray());
    }

    public void SetupWithTeam(TeamController teamController, int desiredTeamNumber = 0)
    {
        if (_teamNumbers.Get(desiredTeamNumber))
        {
            teamController.SetTeamNumber(desiredTeamNumber);
            return;
        }

        for (int i = 0; i < maxTeams; i++)
        {
            if (_teamNumbers.Get(i) == false) continue;
            
            _teamNumbers.Set(i, true);
            teamController.SetTeamNumber(i + 1);
            return;
        }
        
        Debug.LogError("No team numbers available");
    }
}

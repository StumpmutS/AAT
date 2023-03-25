using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class TeamManager : NetworkBehaviour
{
    [SerializeField] private int maxTeams;
    
    public static TeamManager Instance { get; private set; }

    [Networked, Capacity(16)] private NetworkArray<NetworkBool> _teamNumbers => default;
    [Networked, Capacity(16)] private NetworkDictionary<int, PlayerRef> _playersByTeamNumber => default;

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

    public static LayerMask GetLayer(int teamNumber) => LayerMask.GetMask($"Team{teamNumber}");

    public void SetPlayerForTeam(int teamNumber, PlayerRef player)
    {
        if (!Runner.IsServer) return;
        
        _playersByTeamNumber.Set(teamNumber, player);
    }
    
    public PlayerRef GetPlayerForTeam(int teamNumber)
    {
        if (_playersByTeamNumber.TryGet(teamNumber, out var player))
        {
            return player;
        }
        return default;
    }

    public void SetupWithTeam(TeamController teamController, int desiredTeamNumber = 0)
    {
        if (desiredTeamNumber > 0 && _teamNumbers.Get(desiredTeamNumber - 1))
        {
            teamController.SetTeamNumber(desiredTeamNumber);
            return;
        }

        for (int i = 0; i < maxTeams; i++)
        {
            if (_teamNumbers.Get(i)) continue;
            
            _teamNumbers.Set(i, true);
            teamController.SetTeamNumber(i + 1);
            return;
        }
        
        Debug.LogError("No team numbers available");
    }
}

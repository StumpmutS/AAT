using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkStartInfo : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<SectorController, PlayerStartingInfo> startingInfoBySector;
    
    public static NetworkStartInfo Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SetupPlayer(NetworkRunner runner, NetworkObject o)
    {
        PlayerRef playerRef = o.InputAuthority;
        var playerSectors = DetermineSectors(runner, playerRef);
        var playerTeam = o.GetComponent<TeamController>();
        TeamManager.Instance.SetupWithTeam(playerTeam);
        
        var player = o.GetComponent<Player>();
        player.AddSectors(playerSectors);
        
        foreach (var sector in playerSectors)
        {
            sector.Init(playerTeam.GetTeamNumber());
            startingInfoBySector[sector].SpawnPlotController.Object.AssignInputAuthority(playerRef);
            startingInfoBySector[sector].SpawnPlotController.GetComponent<TeamController>().SetTeamNumber(playerTeam.GetTeamNumber());
            foreach (var kvp in startingInfoBySector[sector].StartingResources)
            {
                player.Resources.Set(kvp.Key, kvp.Value);
            }
        }
    }

    private List<SectorController> DetermineSectors(NetworkRunner runner, PlayerRef newPlayer)
    {
        foreach (var sector in startingInfoBySector.Keys)
        {
            if (!SectorAvailable(runner, newPlayer, sector)) continue;
            return new List<SectorController>() { sector };
        }
        
        Debug.LogError("No sectors available");
        return null;
    }

    private bool SectorAvailable(NetworkRunner runner, PlayerRef newPlayer, SectorController sector)
    {
        foreach (var player in runner.ActivePlayers)
        {
            if (player == newPlayer) continue;
            if (runner.GetPlayerObject(player).GetComponent<Player>().OwnedSectorIds.Contains(sector.Object.Id))
            {
                return false;
            }
        }

        return true;
    }
}

[Serializable]
public class PlayerStartingInfo
{
    [SerializeField] private SpawnPlotController spawnPlotController;
    public SpawnPlotController SpawnPlotController => spawnPlotController;
    [SerializeField] private SerializableDictionary<EResourceType, int> startingResources;
    public SerializableDictionary<EResourceType, int> StartingResources => startingResources;
}
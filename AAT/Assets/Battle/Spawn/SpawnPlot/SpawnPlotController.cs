using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Fusion;
using UnityEngine;
using Utility.Scripts;

public class SpawnPlotController : NetworkBehaviour
{
    [SerializeField] private SectorController sector;
    [SerializeField] private EFaction faction;
    public EFaction Faction => faction;

    private TeamController _team;

    private void Awake()
    {
        _team = GetComponent<TeamController>();
    }

    public override void Spawned()
    {
        if (!Runner.IsServer) return;
        
        sector.OnCaptured += HandleSectorCaptured;
    }

    private void HandleSectorCaptured(int team)
    {
        var player = TeamManager.Instance.GetPlayerForTeam(team);
        if (player == Object.InputAuthority) return;
        Object.AssignInputAuthority(player);
        _team.SetTeamNumber(team);
    }
    
    public void CallSetupSpawner(SpawnerSpawnData spawnerSpawnData)
    {
        RpcSetupSpawner(BuildReferenceContainer.Instance.BuildResourceReference.SOResourcePaths[spawnerSpawnData]);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RpcSetupSpawner(string spawnDataPath)
    {
        if (!Runner.IsServer) return;
        
        var spawnData = Resources.Load<SpawnerSpawnData>(spawnDataPath);
        Runner.Spawn(spawnData.SpawnerPrefab, transform.position, transform.rotation, Object.InputAuthority, SetupSpawner);

        Runner.Despawn(Object);

        void SetupSpawner(NetworkRunner _, NetworkObject o)
        {
            var team = o.GetComponent<TeamController>();
            TeamManager.Instance.SetupWithTeam(team, _team.GetTeamNumber());
        }
    }

    public void Setup(SectorController sector, EFaction faction)
    {
        this.sector = sector;
        this.faction = faction;
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (!runner.IsServer) return;
        
        sector.OnCaptured -= HandleSectorCaptured;
    }
}

using System;
using ExitGames.Client.Photon;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(SelectableController))]
public class SpawnPlotController : NetworkBehaviour
{
    [SerializeField] private SpawnPlotButtonsController spawnPlotUI;
    [SerializeField] private SectorController sector;
    [SerializeField] private EFaction faction;

    private TeamController _team;
    private SelectableController _selectable;

    private void Awake()
    {
        _team = GetComponent<TeamController>();
        _selectable = GetComponent<SelectableController>();
        _selectable.OnSelect.AddListener(Select);
        _selectable.OnDeselect.AddListener(Deselect);
    }

    public override void Spawned()
    {
        if (!Runner.IsServer) return;
        
        sector.OnSectorCaptureChanged += HandleSectorCapture;
    }

    private void HandleSectorCapture(PlayerRef player, int amount)
    {
        if (amount < 99.999f || player == Object.InputAuthority || player == default) return;
        Object.AssignInputAuthority(player);
        var playerTeam = Runner.GetPlayerObject(player).GetComponent<TeamController>();
        _team.SetTeamNumber(playerTeam.GetTeamNumber());
    }

    private void Select()
    {
        if (Object == null || !Object.HasInputAuthority) return;

        spawnPlotUI.DisplayByFaction(faction);
        spawnPlotUI.OnSpawnRequest += CallSetupSpawner;
    }

    private void Deselect()
    {
        if (Object == null || !Object.HasInputAuthority) return;

        spawnPlotUI.OnSpawnRequest -= CallSetupSpawner;
    }
    
    private void CallSetupSpawner(UnitSpawnData unitSpawnData)
    {
        if (!_selectable.Selected) return;
        _selectable.CallDeselectOverrideUICheck();
        RpcSetupSpawner(BuildReferenceContainer.Instance.BuildResourceReference.SOResourcePaths[unitSpawnData]);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RpcSetupSpawner(string spawnDataPath)
    {
        if (!Runner.IsServer) return;
        
        var spawnData = Resources.Load<UnitSpawnData>(spawnDataPath);
        var instantiatedSpawner = Runner.Spawn(spawnData.SpawnerPrefab, transform.position, transform.rotation, Object.InputAuthority, SetupSpawner).GetComponent<SpawnerController>();

        instantiatedSpawner.Init(spawnData, sector);
        Runner.Despawn(Object);

        void SetupSpawner(NetworkRunner _, NetworkObject o)
        {
            var team = o.GetComponent<TeamController>();
            TeamManager.Instance.SetupWithTeam(team, _team.GetTeamNumber());
            var visuals = Runner.Spawn(spawnData.SpawnerVisuals).GetComponent<SpawnerVisualsController>();
            visuals.transform.position = o.transform.position;
            visuals.transform.parent = o.transform;
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
        
        sector.OnSectorCaptureChanged -= HandleSectorCapture;
        spawnPlotUI.OnSpawnRequest -= CallSetupSpawner;
        if (_selectable != null) _selectable.OnSelect.AddListener(Select);
        if (_selectable != null) _selectable.OnDeselect.AddListener(Deselect);
    }
}

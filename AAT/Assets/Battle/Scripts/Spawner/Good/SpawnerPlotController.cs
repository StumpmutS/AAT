using System;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(SelectableController))]
public class SpawnerPlotController : NetworkBehaviour
{
    [SerializeField] private GameObject upgradesUIContainer;
    [SerializeField] private SectorController sector;
    [SerializeField] private EFaction faction;

    private TeamController _team;
    private SelectableController _selectable;
    
    public event Action<SpawnerPlotController, EFaction> OnSpawnerPlotSelect = delegate { };
    public event Action OnSpawnerPlotDeselect = delegate { };

    private void Awake()
    {
        _team = GetComponent<TeamController>();
        _selectable = GetComponent<SelectableController>();
        _selectable.OnSelect += Select;
        _selectable.OnDeselect += Deselect;
    }

    private void Select()
    {
        if (Object == null) return;
        
        var player = Object.Runner.GetPlayerObject(Object.Runner.LocalPlayer).GetComponent<Player>();
        if (!player.OwnedSectors.Contains(sector.Id)) return;
        OnSpawnerPlotSelect.Invoke(this, faction);
    }

    private void Deselect() => OnSpawnerPlotDeselect.Invoke();

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcSetupSpawner(string spawnDataPath)
    {
        if (!Object.HasStateAuthority) return;
        
        var spawnData = (UnitSpawnData) Resources.Load(spawnDataPath);
        var instantiatedSpawner = Runner.Spawn(spawnData.SpawnerPrefab, transform.position, transform.rotation, Object.InputAuthority, onBeforeSpawned: SetSpawnerTeam).GetComponent<SpawnerController>();

        instantiatedSpawner.Setup(spawnData, sector, upgradesUIContainer);
        Runner.Despawn(Object);

        void SetSpawnerTeam(NetworkRunner _, NetworkObject o)
        {
            var team = o.GetComponent<TeamController>();
            TeamManager.Instance.SetupWithTeam(team, _team.GetTeamNumber());
        }
    }

    public void Setup(GameObject upgradesUI, SectorController sector, EFaction faction)
    {
        upgradesUIContainer = upgradesUI;
        this.sector = sector;
        this.faction = faction;
    }
    
    public void CallSelect() => _selectable.CallSelect();

    public void CallDeselect() => _selectable.CallDeselect();
}

using System;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(SelectableController))]
public class SpawnerPlotController : SimulationBehaviour
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
        var player = Object.Runner.GetPlayerObject(Object.Runner.LocalPlayer).GetComponent<Player>();
        if (!player.OwnedSectors.Contains(sector)) return;
        OnSpawnerPlotSelect.Invoke(this, faction);
    }

    private void Deselect() => OnSpawnerPlotDeselect.Invoke();

    public void SetupSpawner(UnitSpawnData spawnData)
    {
        SpawnerController instantiatedSpawner = Runner.Spawn(spawnData.SpawnerPrefab, transform.position, transform.rotation, 
            onBeforeSpawned: (_, o) => 
                TeamManager.Instance.SetupWithTeam(o.GetComponent<TeamController>(), _team.GetTeamNumber())).GetComponent<SpawnerController>();
        
        SpawnerManager.Instance.AddSpawnerPlot(instantiatedSpawner);
        instantiatedSpawner.Setup(spawnData, sector, upgradesUIContainer);
        Runner.Despawn(this.Object);
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

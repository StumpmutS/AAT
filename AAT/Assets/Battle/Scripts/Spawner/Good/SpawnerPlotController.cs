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
        if (Object == null) return;
        
        var player = Object.Runner.GetPlayerObject(Object.Runner.LocalPlayer).GetComponent<Player>();
        if (!player.OwnedSectors.Contains(sector)) return;
        OnSpawnerPlotSelect.Invoke(this, faction);
    }

    private void Deselect() => OnSpawnerPlotDeselect.Invoke();

    public void SetupSpawner(UnitSpawnData spawnData)
    {
        //TODO: null ref as client, TRY MAKING SPAWNPLOTMANAGER SIMULATIONBEHAVIOR
        var prefab = spawnData.SpawnerPrefab;
        var networkObject = Runner.Spawn(prefab, transform.position, transform.rotation, onBeforeSpawned: SetSpawnerTeam); 
        //^this returns null, ensure all calls run on simulationBehavior from simulation loop.
        //If still nothing, ensure this method is called on server as well, as it could be problem with Spawn call on clients not actually doing anything <- MOST LIKELY
        
        var instantiatedSpawner = networkObject.GetComponent<SpawnerController>();

        SpawnerManager.Instance.AddSpawnerPlot(instantiatedSpawner);
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

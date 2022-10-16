using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class UnitController : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnSectorChange))] public NetworkId SectorId { get; set; }
    public SectorController Sector { get; private set; }
    public static void OnSectorChange(Changed<UnitController> changed)
    {
        changed.Behaviour.Sector = changed.Behaviour.Runner.FindObject(changed.Behaviour.SectorId).GetComponent<SectorController>();
    }
    [Networked] public NetworkBool IsDead { get; set; }
    
    [SerializeField] private Collider[] colliders;
    public Collider[] Colliders => colliders;
    [SerializeField] private PoolingObject unitVisuals;
    public PoolingObject UnitVisuals => unitVisuals;
    [SerializeField] private UnitManager unitManager;
    
    public TeamController Team { get; private set; }
    public bool NetworkSelected { get; private set; }
    public SelectableController Selectable { get; private set; }
    public StatsManager Stats { get; private set; }

    private UnitDeathController _unitDeathController;
    
    public event Action<UnitController> OnDeath = delegate { };
    
    private void Awake()
    {
        Team = GetComponent<TeamController>();
        Selectable = GetComponent<SelectableController>();
        Selectable.OnSelect.AddListener(Select);
        Selectable.OnDeselect.AddListener(Deselect);
        
        Stats = GetComponent<StatsManager>();
        _unitDeathController = GetComponent<UnitDeathController>();
        _unitDeathController.OnUnitDeath += HandleUnitDeath;
    }

    private void Start()
    {
        if (unitManager == null) unitManager = UnitManager.Instance;
        unitManager.AddUnit(this);
    }

    public void SetSector(SectorController sector)
    {
        if (sector == Sector) return;
        if (Sector != null) Sector.RemoveUnit(this);
        Sector = sector;
        SectorId = sector.Object.Id;
        Sector.AddUnit(this);
    }

    public void Init(int teamNumber, SectorController sector)
    {
        Team.SetTeamNumber(teamNumber);
        SetSector(sector);
    }

    private void HandleUnitDeath()
    {
        Selectable.CallDeselect();
        if (!Runner.IsServer) return;
        
        if (Sector != null) Sector.RemoveUnit(this);
        IsDead = true;
        OnDeath.Invoke(this);
    }

    public void ModifyStats(BaseUnitStatsData baseUnitStatsDataInfo, bool add = true)
    {
        Stats.ModifyStats(baseUnitStatsDataInfo, add);
    }

    private void Select()
    {
        if (!Object.HasInputAuthority) return;
        RpcSetSelected();
        UnitManager.Instance.AddSelectedUnit(this);
    }

    private void Deselect()
    {
        if (!Object.HasInputAuthority) return;
        RpcSetDeselected();
        UnitManager.Instance.RemoveSelectedUnit(this);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RpcSetSelected()
    {
        NetworkSelected = true;
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RpcSetDeselected()
    {
        NetworkSelected = false;
    }

    private void OnDestroy()
    {
        Selectable.OnSelect.RemoveListener(Select);
        Selectable.OnDeselect.RemoveListener(Deselect);
    }
}

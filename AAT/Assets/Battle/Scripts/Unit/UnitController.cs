using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class UnitController : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnSectorChange))] public NetworkId SectorId { get; set; }
    public static void OnSectorChange(Changed<UnitController> changed)
    {
        changed.Behaviour.Sector = changed.Behaviour.Runner.FindObject(changed.Behaviour.SectorId).GetComponent<SectorController>();
    }
    public SectorController Sector { get; private set; }
    [Networked] public NetworkBool IsDead { get; set; }
    
    [SerializeField] private List<EInteractableType> interactableTypes;
    public List<EInteractableType> InteractableTypes => interactableTypes;

    [SerializeField] private bool chaseState;
    public bool ChaseState => chaseState;
    [SerializeField] private bool patrolState;
    public bool PatrolState => patrolState;
    [SerializeField] private List<Vector3> _patrolPoints;
    public List<Vector3> PatrolPoints => _patrolPoints;
    [SerializeField] private Collider[] colliders;
    public Collider[] Colliders => colliders;
    [SerializeField] private PoolingObject unitVisuals;
    public PoolingObject UnitVisuals => unitVisuals;
    [SerializeField] private UnitManager unitManager;
    
    public TeamController Team { get; private set; }
    public bool NetworkSelected { get; private set; }
    public OutlineSelectableController OutlineSelectable { get; private set; }
    public UnitStatsModifierManager Stats { get; private set; }
    public UnitGroupController UnitGroup { get; private set; }

    private UnitDeathController _unitDeathController;
    
    public event Action<UnitController> OnDeath = delegate { };
    
    private void Awake()
    {
        Team = GetComponent<TeamController>();
        OutlineSelectable = GetComponent<OutlineSelectableController>();
        OutlineSelectable.OnSelect += Select;
        OutlineSelectable.OnDeselect += Deselect;
        
        Stats = GetComponent<UnitStatsModifierManager>();
        _unitDeathController = GetComponent<UnitDeathController>();
        _unitDeathController.OnUnitDeath += UnitDeath;
    }

    private void Start()
    {
        if (unitManager == null) unitManager = UnitManager.Instance;
        unitManager.AddUnit(this);
    }

    public void Init(int teamNumber, SectorController sector, UnitGroupController group)
    {
        Team.SetTeamNumber(teamNumber);
        SetSector(sector);
        SetGroup(group);
    }

    private void UnitDeath()
    {
        OutlineSelectable.CallDeselect();
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

    #region Setters
    public void SetSector(SectorController sector)
    {
        if (sector == Sector) return;
        if (Sector != null) Sector.RemoveUnit(this);
        Sector = sector;
        SectorId = sector.Object.Id;
        Sector.AddUnit(this);
    }

    public void SetGroup(UnitGroupController group)
    {
        if (UnitGroup != null) UnitGroup.RemoveUnit(this);
        UnitGroup = group;
        UnitGroup.AddUnit(this);
    }

    public void SetPatrolPoints(List<Vector3> patrolPoints)
    {
        _patrolPoints = patrolPoints;
        patrolState = true;
    }

    public void SetChaseState(bool value)
    {
        chaseState = value;
    }
    #endregion
}

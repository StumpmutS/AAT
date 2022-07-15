using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class UnitController : SimulationBehaviour
{

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
    [SerializeField] private UnitManager unitManager;

    public TeamController Team { get; private set; }
    public OutlineSelectableController OutlineSelectable { get; private set; }
    public UnitStatsModifierManager Stats { get; private set; }
    public PoolingObject UnitVisuals { get; private set; }
    public UnitGroupController UnitGroup { get; private set; }
    public int GroupIndex { get; private set; }
    public SectorController Sector { get; private set; }
    public bool IsDead { get; private set; }

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
        UnitVisuals = GetComponent<PoolingObject>();
        if (unitManager == null) unitManager = UnitManager.Instance;
        unitManager.AddUnit(this);
    }

    private void UnitDeath()
    {
        OutlineSelectable.CallDeselect();
        if (Sector != null) Sector.ModifySectorPower(-Stats.CalculatePower());
        IsDead = true;
        OnDeath.Invoke(this);
    }

    public void ModifyStats(BaseUnitStatsData baseUnitStatsDataInfo, bool add = true)
    {
        Stats.ModifyStats(baseUnitStatsDataInfo, add);
    }

    private void Select() => UnitManager.Instance.AddSelectedUnit(this);

    private void Deselect() => UnitManager.Instance.RemoveSelectedUnit(this);

    #region Setters
    public void SetSector(SectorController sector)
    {
        Sector = sector;
    }

    public void SetGroup(UnitGroupController group, int index)
    {
        UnitGroup = group;
        GroupIndex = index;
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

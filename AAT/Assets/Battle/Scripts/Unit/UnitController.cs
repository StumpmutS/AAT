using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitController : OutlineSelectableController
{
    [FormerlySerializedAs("unitStatsModifierManager")] [SerializeField] private UnitStatsModifierManager stats;
    public UnitStatsModifierManager Stats => stats;
    [SerializeField] private List<EInteractableType> interactableTypes;
    public List<EInteractableType> InteractableTypes => interactableTypes;
    [SerializeField] private UnitDeathController unitDeathController;
    [SerializeField] private PoolingObject unitVisuals;     
    public PoolingObject UnitVisuals => unitVisuals;
    [SerializeField] private bool chaseState;
    public bool ChaseState => chaseState;
    [SerializeField] private bool patrolState;
    public bool PatrolState => patrolState;
    [SerializeField] private List<Vector3> _patrolPoints;
    public List<Vector3> PatrolPoints => _patrolPoints;
    [SerializeField] private Collider[] colliders;
    public Collider[] Colliders => colliders;
    [SerializeField] private UnitManager unitManager;

    public UnitGroupController UnitGroup { get; private set; }
    public SectorController Sector { get; private set; }
    public bool IsDead { get; private set; }

    [SerializeField] private LayerMask enemyLayer;
    public LayerMask EnemyLayer => enemyLayer; //TODO: temp until networked

    public event Action<UnitController> OnDeath = delegate { };
    
    private void Awake()
    {
        unitDeathController.OnUnitDeath += UnitDeath;
        if (unitManager == null) unitManager = UnitManager.Instance;
        unitManager.AddUnit(this);
    }

    private void UnitDeath()
    {
        CallDeselect();
        if (Sector != null) Sector.RemoveUnit(this);
        IsDead = true;
        OnDeath.Invoke(this);
    }

    public void ModifyStats(BaseUnitStatsData baseUnitStatsDataInfo, bool add = true)
    {
        stats.ModifyStats(baseUnitStatsDataInfo, add);
    }

    protected override void Select()
    {
        base.Select();
        UnitManager.Instance.AddSelectedUnit(this);
    }

    protected override void Deselect()
    {
        base.Deselect();
        UnitManager.Instance.RemoveSelectedUnit(this);
    }

    #region Setters
    public void SetSector(SectorController sector)
    {
        Sector = sector;
    }

    public void SetGroup(UnitGroupController group)
    {
        UnitGroup = group;
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

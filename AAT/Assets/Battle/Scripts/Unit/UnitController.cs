using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : OutlineSelectableController
{
    [SerializeField] private UnitStatsModifierManager unitStatsModifierManager;
    public UnitStatsModifierManager UnitStatsModifierManager => unitStatsModifierManager;
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
    public SectorController SectorController { get; private set; }
    public bool IsDead { get; private set; }

    public event Action<UnitController> OnDeath = delegate { };
    public event Action OnInteractionFinished = delegate { };

    private void Awake()
    {
        unitDeathController.OnUnitDeath += UnitDeath;
        if (unitManager == null) unitManager = UnitManager.Instance;
        unitManager.AddUnit(this);
    }

    private void UnitDeath()
    {
        CallDeselect();
        if (SectorController != null) SectorController.RemoveUnit(this);
        IsDead = true;
        OnDeath.Invoke(this);
    }

    public void ModifyStats(BaseUnitStatsData baseUnitStatsDataInfo, bool add = true)
    {
        unitStatsModifierManager.ModifyStats(baseUnitStatsDataInfo, add);
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
        SectorController = sector;
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

    public void Interact(InteractableController interactable, Action<UnitController> callback)
    {
        //TODO: walk to interactable
        callback.Invoke(this);
    }

    public void FinishInteraction()
    {
        OnInteractionFinished.Invoke();
    }
}

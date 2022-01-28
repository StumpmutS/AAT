using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AATAgentController))]
public class AIPathfinder : MonoBehaviour
{
    [SerializeField] private UnitController unitController;
    [SerializeField] private LayerMask enemyTeamLayer;
    [SerializeField] private UnitStatsModifierManager unitDataManager;
    [SerializeField] protected UnitAnimationController unitAnimationController;
    [SerializeField] private AbilityHandler abilityHandler;

    protected float _movementSpeed => unitDataManager.CurrentUnitStatsData[EUnitFloatStats.MovementSpeed];
    private float _sightRange => unitDataManager.CurrentUnitStatsData[EUnitFloatStats.SightRange];
    private float _attackRange => unitDataManager.CurrentUnitStatsData[EUnitFloatStats.AttackRange];
    private bool _chaseEnabled => unitController.ChaseState;
    private bool _patrolEnabled => unitController.PatrolState;
    private List<Vector3> _patrolPoints => unitController.PatrolPoints;
    
    public event Action<Collider> OnChase = delegate { };
    public event Action OnChaseStart = delegate { };
    public event Action<Collider> OnAttack = delegate { };
    public event Action OnAttackStart = delegate { };
    public event Action<List<Vector3>> OnPatrol = delegate { };
    public event Action OnPatrolStart = delegate { };
    public event Action OnNoAIState = delegate { };

    protected AATAgentController _agent;
    private bool _patrolling;
    private Collider _currentAttackTarget;
    private bool _usingAbility;
    protected bool _active = true;

    protected virtual void Awake()
    {
        _agent = GetComponent<AATAgentController>();
        if (abilityHandler != null)
            abilityHandler.OnAbilityUsed += SetAbilityUsage;
        unitDataManager.OnRefreshStats += RefreshMovementSpeed;
    }

    private void Start()
    {
        _agent.SetSpeed(_movementSpeed);
    }

    private void Update()
    {
        if (!_active) return;
        
        if (_usingAbility)
        {
            OnNoAIState.Invoke();
            return;
        }

        if (CheckRange(_attackRange, out var attackTarget))
        {
            _currentAttackTarget = attackTarget;
            Attack(attackTarget);
        }
        else if (_chaseEnabled)
        {
            if (CheckRange(_sightRange, out var chaseTarget))
            {
                Chase(chaseTarget);
            }
            else if (_patrolEnabled && !_patrolling)
            {
                Patrol();
            }
            else
            {
                NoAIState();
            }
        }
        else if (_patrolEnabled && !_patrolling)
        {
            Patrol();
        }
        else
        {
            NoAIState();
        }
    }
    
    public bool CheckRange(float range, out Collider target)
    {
        if (Physics.CheckSphere(transform.position, range, enemyTeamLayer))
        {
            target = FindTarget(range);
            return true;
        }
        target = null;
        return false;
    }

    private Collider FindTarget(float range)
    {
        var hits = new Collider[50];
        var position = transform.position;
        Physics.OverlapSphereNonAlloc(position, range, hits, enemyTeamLayer);
        var target = DistanceCompare.FindClosestCollider(hits, position, _currentAttackTarget);
        _currentAttackTarget = null;
        return target;
    }

    //dont do animation stuff in here

    protected virtual void Attack(Collider target)
    {
        if (unitAnimationController != null)
            unitAnimationController.SetMovement(0);
        _patrolling = false;
        OnAttackStart.Invoke();
        OnAttack.Invoke(target);
    }

    protected virtual void Chase(Collider target)
    {
        if (unitAnimationController != null)
            unitAnimationController.SetMovement(_movementSpeed);
        _patrolling = false;
        OnChaseStart.Invoke();
        OnChase.Invoke(target);
    }

    protected virtual void Patrol()
    {
        if (unitAnimationController != null)
            unitAnimationController.SetMovement(_movementSpeed);
        _patrolling = true;
        OnPatrolStart.Invoke();
        OnPatrol.Invoke(_patrolPoints);
    }

    protected virtual void NoAIState()
    {
        if (unitAnimationController != null)
            unitAnimationController.SetMovement(0);
        OnNoAIState.Invoke();
    }

    private void RefreshMovementSpeed()
    {
        _agent.SetSpeed(_movementSpeed);
    }

    private void SetAbilityUsage(bool value)
    {
        if (value) _agent.DisableAgent(this);
        else _agent.EnableAgent(this);
        _usingAbility = value;
    }
    
    public void Activate()
    {
        _active = true;
    }

    public virtual void Deactivate()
    {
        _active = false;
    }
}

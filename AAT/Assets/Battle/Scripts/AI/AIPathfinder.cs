using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIPathfinder : MonoBehaviour
{
    [SerializeField] private UnitController unitController;
    [SerializeField] private LayerMask enemyTeamLayer;
    [SerializeField] private UnitStatsModifierManager unitDataManager;
    [SerializeField] protected UnitAnimationController unitAnimationController;
    [SerializeField] private AbilityHandler abilityHandler;

    protected float movementSpeed => unitDataManager.CurrentUnitStatsData[EUnitFloatStats.MovementSpeed];
    private float sightRange => unitDataManager.CurrentUnitStatsData[EUnitFloatStats.SightRange];
    private float attackRange => unitDataManager.CurrentUnitStatsData[EUnitFloatStats.AttackRange];
    private bool chaseEnabled => unitController.ChaseState;
    private bool patrolEnabled => unitController.PatrolState;
    private List<Vector3> _patrolPoints => unitController.PatrolPoints;

    public event Action OnActivation = delegate { };
    public event Action OnDeactivation = delegate { };
    
    public event Action<GameObject> OnChase = delegate { };
    public event Action OnChaseStart = delegate { };
    public event Action<GameObject> OnAttack = delegate { };
    public event Action OnAttackStart = delegate { };
    public event Action<List<Vector3>> OnPatrol = delegate { };
    public event Action OnPatrolStart = delegate { };
    public event Action OnNoAIState = delegate { };

    protected NavMeshAgent _agent;
    private bool _patrolling = false;
    private GameObject _currentAttackTarget = null;
    private bool _usingAbility = false;
    protected bool _active = true;

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (abilityHandler != null)
            abilityHandler.OnAbilityUsed += SetAbilityUsage;
        unitDataManager.OnRefreshStats += RefreshMovementSpeed;
    }

    private void Start()
    {
        _agent.speed = movementSpeed;
    }

    private void Update()
    {
        if (!_active) return;
        
        if (_usingAbility)
        {
            OnNoAIState.Invoke();
            return;
        }

        if (CheckRange(attackRange, out GameObject attackTarget))
        {
            _currentAttackTarget = attackTarget;
            Attack(attackTarget);
        }
        else if (chaseEnabled)
        {
            if (CheckRange(sightRange, out GameObject chaseTarget))
            {
                Chase(chaseTarget);
            }
            else if (patrolEnabled && !_patrolling)
            {
                Patrol();
            }
            else
            {
                NoAIState();
            }
        }
        else if (patrolEnabled && !_patrolling)
        {
            Patrol();
        }
        else
        {
            NoAIState();
        }
    }
    
    public bool CheckRange(float range, out GameObject target)
    {
        if (Physics.CheckSphere(transform.position, range, enemyTeamLayer))
        {
            target = FindTarget(range);
            return true;
        }
        target = null;
        return false;
    }

    private GameObject FindTarget(float range)
    {
        GameObject target = null;
        float targetDistanceSquared = Mathf.Infinity;
        Collider[] hits = new Collider[50];
        Physics.OverlapSphereNonAlloc(transform.position, range, hits, enemyTeamLayer);
        foreach(Collider collider in hits)
        {
            if (collider != null)
            {
                GameObject potentialTarget = collider.gameObject;
                if (potentialTarget == _currentAttackTarget) return _currentAttackTarget;

                float newDistanceSquared = (transform.position - potentialTarget.transform.position).sqrMagnitude;
                if (newDistanceSquared < targetDistanceSquared)
                {
                    targetDistanceSquared = newDistanceSquared;
                    target = potentialTarget;
                }
            }
        }
        _currentAttackTarget = null;
        return target;
    }

    //dont do animation stuff in here

    protected virtual void Attack(GameObject target)
    {
        if (unitAnimationController != null)
            unitAnimationController.SetMovement(0);
        _patrolling = false;
        OnAttackStart.Invoke();
        OnAttack.Invoke(target);
    }

    protected virtual void Chase(GameObject target)
    {
        if (unitAnimationController != null)
            unitAnimationController.SetMovement(movementSpeed);
        _patrolling = false;
        OnChaseStart.Invoke();
        OnChase.Invoke(target);
    }

    protected virtual void Patrol()
    {
        if (unitAnimationController != null)
            unitAnimationController.SetMovement(movementSpeed);
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
        _agent.speed = movementSpeed;
    }

    private void SetAbilityUsage(bool value)
    {
        _agent.enabled = !value;
        _usingAbility = value;
    }
    
    public virtual void Activate()
    {
        OnActivation.Invoke();
        _active = true;
    }

    public virtual void Deactivate()
    {
        OnDeactivation.Invoke();
        _active = false;
    }
}

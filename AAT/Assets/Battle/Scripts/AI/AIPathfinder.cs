using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIPathfinder : MonoBehaviour
{
    [SerializeField] private LayerMask enemyTeamLayer;
    [SerializeField] private bool chaseEnabled;
    [SerializeField] private bool patrolEnabled;
    [SerializeField] private List<Vector3> patrolPoints;
    [SerializeField] private UnitStatsModifierManager unitDataManager;
    [SerializeField] protected UnitAnimationController unitAnimationController;

    protected float movementSpeed => unitDataManager.CurrentUnitStatsData.MovementSpeed;
    private float sightRange => unitDataManager.CurrentUnitStatsData.SightRange;
    private float attackRange => unitDataManager.CurrentUnitStatsData.AttackRange;
    
    public event Action<GameObject> OnChase = delegate { };
    public event Action OnChaseStart = delegate { };
    public event Action<GameObject> OnAttack = delegate { };
    public event Action OnAttackStart = delegate { };
    public event Action<List<Vector3>> OnPatrol = delegate { };
    public event Action OnPatrolStart = delegate { };
    public event Action OnNoAIState = delegate { };

    protected NavMeshAgent agent;
    private bool patrolling = false;
    private GameObject _currentAttackTarget = null;
    

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        unitDataManager.OnRefreshStats += RefreshMovementSpeed;
    }

    private void Start()
    {
        agent.speed = movementSpeed;
    }

    private void Update()
    {
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
            else if (patrolEnabled && !patrolling)
            {
                Patrol();
            }
            else
            {
                NoAIState();
            }
        }
        else if (patrolEnabled && !patrolling)
        {
            Patrol();
        }
        else
        {
            NoAIState();
        }
    }
    
    private bool CheckRange(float range, out GameObject target)
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

    private void Attack(GameObject target)
    {
        if (unitAnimationController != null)
            unitAnimationController.SetMovement(0);
        patrolling = false;
        OnAttackStart.Invoke();
        OnAttack.Invoke(target);
    }

    private void Chase(GameObject target)
    {
        if (unitAnimationController != null)
            unitAnimationController.SetMovement(movementSpeed);
        patrolling = false;
        OnChaseStart.Invoke();
        OnChase.Invoke(target);
    }

    private void Patrol()
    {
        if (unitAnimationController != null)
            unitAnimationController.SetMovement(movementSpeed);
        patrolling = true;
        OnPatrolStart.Invoke();
        OnPatrol.Invoke(patrolPoints);
    }

    private void NoAIState()
    {
        if (unitAnimationController != null)
            unitAnimationController.SetMovement(0);
        OnNoAIState.Invoke();
    }

    private void RefreshMovementSpeed()
    {
        agent.speed = movementSpeed;
    }
}

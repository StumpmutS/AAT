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

    private float movementSpeed => unitDataManager.CurrentUnitStatsData.MovementSpeed;
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
    

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.speed = movementSpeed;
    }

    private void Update()
    {
        if (CheckRange(attackRange, out GameObject attackTarget))
        {
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
        }
        else if (patrolEnabled && !patrolling)
        {
            Patrol();
        }
        else
        {
            OnNoAIState.Invoke();
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
                float newDistanceSquared = (transform.position - collider.transform.position).sqrMagnitude;
                if (newDistanceSquared < targetDistanceSquared)
                {
                    targetDistanceSquared = newDistanceSquared;
                    target = collider.gameObject;
                }
            }
        }
        return target;
    }

    private void Attack(GameObject target)
    {
        patrolling = false;
        OnAttackStart.Invoke();
        OnAttack.Invoke(target);
    }

    private void Chase(GameObject target)
    {
        patrolling = false;
        OnChaseStart.Invoke();
        OnChase.Invoke(target);
    }

    private void Patrol()
    {
        patrolling = true;
        OnPatrolStart.Invoke();
        OnPatrol.Invoke(patrolPoints);
    }
}

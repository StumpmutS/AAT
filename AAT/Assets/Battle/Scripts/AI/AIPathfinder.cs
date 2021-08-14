using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathfinder : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] private LayerMask enemyTeamLayer;
    [SerializeField] private bool chaseEnabled;
    [SerializeField] private bool patrolEnabled;
    [SerializeField] private List<Vector3> patrolPoints;
    [SerializeField] private UnitStatsData unitData;

    private float movementSpeed => unitData.MovementSpeed;
    private float sightRange => unitData.SightRange;
    private float attackRange => unitData.AttackRange;
    
    public event Action<GameObject> OnChase = delegate { };
    public event Action OnChaseStart = delegate { };
    public event Action<GameObject> OnAttack = delegate { };
    public event Action OnAttackStart = delegate { };
    public event Action<List<Vector3>> OnPatrol = delegate { };
    public event Action OnPatrolStart = delegate { };

    private bool patrolling = false;

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

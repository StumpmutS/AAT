using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathfinder : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] private LayerMask enemyTeamLayer;
    [SerializeField] private bool chase;
    [SerializeField] private bool patrol;
    [SerializeField] private List<Vector3> patrolPoints;
    [SerializeField] private UnitStatsData unitData;

    private float movementSpeed => unitData.MovementSpeed;
    private float sightRange => unitData.SightRange;
    private float attackRange => unitData.AttackRange;
    
    public event Action<GameObject> OnChase = delegate { };
    public event Action<GameObject> OnAttack = delegate { };
    public event Action<List<Vector3>> OnPatrol = delegate { };

    private void Start()
    {
        agent.speed = movementSpeed;
    }

    private void Update()
    {
        if (CheckRange(attackRange, out GameObject attackTarget))
        {
            Debug.Log("attack");
            Attack(attackTarget);
        }
        else if (chase)
        {
            if (CheckRange(sightRange, out GameObject chaseTarget))
            {
                Chase(chaseTarget);
            }
        }
        else if (patrol)
        {
            Patrol();
        }
        else
        {
            agent.speed = movementSpeed;
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
        OnAttack.Invoke(target);
    }

    private void Chase(GameObject target)
    {
        OnChase.Invoke(target);
    }

    private void Patrol()
    {
        if (!patrol) return;
        OnPatrol.Invoke(patrolPoints);
    }
}

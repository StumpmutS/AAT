using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathfinder : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] private float movementSpeed;
    public float MovementSpeed => movementSpeed;
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyTeamLayer;

    public event Action<GameObject> OnChase = delegate { };
    public event Action<GameObject> OnAttack = delegate { };
    public event Action<List<Vector3>> OnPatrol = delegate { };

    public bool chase;
    public bool patrol;
    public List<Vector3> patrolPoints;

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
        float targetDistance = Mathf.Infinity;
        Collider[] hits = new Collider[50];
        Physics.OverlapSphereNonAlloc(transform.position, range, hits, enemyTeamLayer);
        foreach(Collider collider in hits)
        {
            if (collider != null)
            {
                float newDistance = Vector3.Distance(transform.position, collider.transform.position);
                if (newDistance < targetDistance)
                {
                    targetDistance = newDistance;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathfinder), typeof(NavMeshAgent))]
public class BasePatrolController : MonoBehaviour
{
    private NavMeshAgent agent;
    private AIPathfinder AI;
    private bool patrolling = false;
    private List<Vector3> _patrolPoints;
    private int currentPatrolPointIndex;

    private void Awake()
    {
        AI = GetComponent<AIPathfinder>();
        agent = GetComponent<NavMeshAgent>();
        AI.OnPatrol += Patrol;
        AI.OnChaseStart += StopPatrol;
        AI.OnAttackStart += StopPatrol;
    }

    private void Update()
    {
        if (patrolling)
        {
            if (agent.remainingDistance < .1f)
            {
                NextPatrolPoint();
            }
        }
    }

    private void Patrol(List<Vector3> patrolPoints)
    {
        _patrolPoints = patrolPoints;
        SetDestination(0);
        patrolling = true;

    }

    private void StopPatrol()
    {
        patrolling = false;
    }

    private void NextPatrolPoint()
    {
        if (currentPatrolPointIndex < _patrolPoints.Count - 1)
        {
            SetDestination(currentPatrolPointIndex + 1);
        }
        else
        {
            SetDestination(0);
        }
    }

    private void SetDestination(int patrolPointIndex)
    {
        agent.SetDestination(_patrolPoints[patrolPointIndex]);
        currentPatrolPointIndex = patrolPointIndex;
    }
}

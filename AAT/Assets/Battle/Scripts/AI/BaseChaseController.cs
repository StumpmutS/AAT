using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathfinder), typeof(NavMeshAgent))]
public class BaseChaseController : MonoBehaviour
{
    [SerializeField] private float chaseSpeedPercentMultiplier;

    private float baseSpeed;
    private AIPathfinder AI;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        AI = GetComponent<AIPathfinder>();
        AI.OnChase += Chase;
        baseSpeed = AI.MovementSpeed;
    }

    protected virtual void Chase(GameObject target)
    {
        agent.speed = baseSpeed * (chaseSpeedPercentMultiplier / 100);
        agent.SetDestination(target.transform.position);
    }
}

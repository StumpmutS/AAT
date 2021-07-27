using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathfinder))]
public class BaseChaseController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float chaseSpeedPercentMultiplier;

    private float baseSpeed;
    private AIPathfinder AI;

    private void Start()
    {
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

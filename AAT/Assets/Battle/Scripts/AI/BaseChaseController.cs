using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathfinder), typeof(NavMeshAgent))]
public class BaseChaseController : MonoBehaviour
{
    [SerializeField] private UnitStatsUpgradeManager unitDataManager;

    private float baseSpeed => unitDataManager.CurrentUnitStatsData.MovementSpeed;
    private float chaseSpeedPercentMultiplier => unitDataManager.CurrentUnitStatsData.ChaseSpeedPercentMultiplier;

    private AIPathfinder AI;
    private NavMeshAgent agent;

    private void Awake()
    {
        AI = GetComponent<AIPathfinder>();
        agent = GetComponent<NavMeshAgent>();
        AI.OnChase += Chase;
        AI.OnPatrolStart += StopChase;
        AI.OnAttackStart += StopChase;
        AI.OnNoAIState += StopChase;
    }

    protected virtual void Chase(GameObject target)
    {
        agent.speed = baseSpeed * (chaseSpeedPercentMultiplier / 100);
        agent.SetDestination(target.transform.position);
    }

    private void StopChase()
    {
        agent.speed = baseSpeed;
    }
}

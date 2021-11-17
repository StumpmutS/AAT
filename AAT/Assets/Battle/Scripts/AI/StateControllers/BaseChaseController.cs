using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathfinder), typeof(NavMeshAgent))]
public class BaseChaseController : MonoBehaviour
{
    [SerializeField] protected UnitStatsModifierManager unitDataManager;

    protected float moveSpeed => unitDataManager.CurrentUnitStatsData.UnitFloatStats[EUnitFloatStats.MovementSpeed];
    protected float chaseSpeedPercentMultiplier => unitDataManager.CurrentUnitStatsData.UnitFloatStats[EUnitFloatStats.ChaseSpeedPercentMultiplier];

    protected AIPathfinder AI;
    protected NavMeshAgent agent;

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
        unitDataManager.ModifyFloatStat(EUnitFloatStats.MovementSpeed, (moveSpeed * chaseSpeedPercentMultiplier / 100) - moveSpeed);
        agent.speed = moveSpeed;
        agent.SetDestination(target.transform.position);
    }

    protected virtual void StopChase()
    {
        unitDataManager.ModifyFloatStat(EUnitFloatStats.MovementSpeed, -((moveSpeed * chaseSpeedPercentMultiplier / 100) - moveSpeed));
        agent.speed = moveSpeed;
    }
}

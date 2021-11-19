using System;
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
    private float _alteredMoveSpeed;

    protected AIPathfinder AI;
    protected NavMeshAgent agent;
    private bool _stopped = true;

    private void Awake()
    {
        AI = GetComponent<AIPathfinder>();
        agent = GetComponent<NavMeshAgent>();
        AI.OnChase += Chase;
        AI.OnPatrolStart += StopChase;
        AI.OnAttackStart += StopChase;
        AI.OnNoAIState += StopChase;
        
    }

    private void Start()
    {
        _alteredMoveSpeed = moveSpeed * chaseSpeedPercentMultiplier / 100 - moveSpeed;
    }

    protected virtual void Chase(GameObject target)
    {
        if (_stopped)
        {
            unitDataManager.ModifyFloatStat(EUnitFloatStats.MovementSpeed, _alteredMoveSpeed);
            _stopped = false;
        }

        Vector3 targetPos = target.transform.position;
        agent.SetDestination(new Vector3(targetPos.x, transform.position.y, targetPos.z));
        agent.speed = moveSpeed;
    }

    protected virtual void StopChase()
    {
        if (!_stopped)
        {
            unitDataManager.ModifyFloatStat(EUnitFloatStats.MovementSpeed, -_alteredMoveSpeed);
            _stopped = true;
        }

        agent.speed = moveSpeed;
    }
}

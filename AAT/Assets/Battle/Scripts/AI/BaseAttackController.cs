using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathfinder), typeof(NavMeshAgent))]
public class BaseAttackController : MonoBehaviour
{
    [SerializeField] protected UnitStatsUpgradeManager unitDataManager;

    private float damage => unitDataManager.CurrentUnitStatsData.Damage;
    private float attackSpeedPercent => unitDataManager.CurrentUnitStatsData.AttackSpeedPercent;
    private AIPathfinder AI;
    private NavMeshAgent agent;
    private bool canAttack;

    private void Awake()
    {
        canAttack = true;
        AI = GetComponent<AIPathfinder>();
        AI.OnAttack += CallAttack;
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void CallAttack(GameObject target)
    {
        if (canAttack)
        {
            Attack(target);
            StartCoroutine(StartAttackTimer());
        }
    }

    private IEnumerator StartAttackTimer()
    {
        canAttack = false;
        yield return new WaitForSeconds(100 / attackSpeedPercent);
        canAttack = true;
    }

    protected virtual void Attack(GameObject target)
    {
        transform.LookAt(target.transform);
        agent.SetDestination(transform.position);
        target.GetComponent<IHealth>().ModifyHealth(-Mathf.Abs(damage));
    }
}

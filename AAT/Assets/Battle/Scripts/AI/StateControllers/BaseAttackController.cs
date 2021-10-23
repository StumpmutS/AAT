using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathfinder), typeof(NavMeshAgent))]
public class BaseAttackController : MonoBehaviour
{
    [SerializeField] protected UnitStatsModifierManager unitDataManager;

    private float damage => unitDataManager.CurrentUnitStatsData.UnitFloatStats[EUnitFloatStats.Damage];
    private float critChancePercent => unitDataManager.CurrentUnitStatsData.UnitFloatStats[EUnitFloatStats.CritChancePercent];
    private float critMultiplierPercent => unitDataManager.CurrentUnitStatsData.UnitFloatStats[EUnitFloatStats.CritMultiplierPercent];
    private float attackSpeedPercent => unitDataManager.CurrentUnitStatsData.UnitFloatStats[EUnitFloatStats.AttackSpeedPercent];

    private AIPathfinder AI;
    private NavMeshAgent agent;
    private bool canAttack;

    protected virtual void Awake()
    {
        canAttack = true;
        AI = GetComponent<AIPathfinder>();
        AI.OnAttack += CallAttack;
        agent = GetComponent<NavMeshAgent>();
    }

    public void CallAttack(GameObject target)
    {
        transform.LookAt(target.transform);
        if (agent.enabled)//ai deactivation
            agent.SetDestination(transform.position);
        if (canAttack)
        {
            CheckCrit(target);
            StartCoroutine(StartAttackTimer());
        }
    }

    private IEnumerator StartAttackTimer()
    {
        canAttack = false;
        yield return new WaitForSeconds(100 / attackSpeedPercent);
        canAttack = true;
    }

    private void CheckCrit(GameObject target)
    {
        if (UnityEngine.Random.Range(0f, 100f) <= critChancePercent)
        {
            CritAttack(target);
        }
        else
        {
            BaseAttack(target);
        }
    }

    protected virtual void BaseAttack(GameObject target)
    {
        target.GetComponent<IHealth>().ModifyHealth(-Mathf.Abs(damage));
    }

    protected virtual void CritAttack(GameObject target)
    {
        target.GetComponent<IHealth>().ModifyHealth(-Mathf.Abs(damage) * (critMultiplierPercent / 100));
    }
}

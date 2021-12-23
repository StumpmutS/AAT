using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathfinder), typeof(AATAgentController))]
public class BaseAttackController : MonoBehaviour
{
    [SerializeField] protected UnitStatsModifierManager unitDataManager;

    protected float damage => unitDataManager.CurrentUnitStatsData[EUnitFloatStats.Damage];
    protected float critChancePercent => unitDataManager.CurrentUnitStatsData[EUnitFloatStats.CritChancePercent];
    protected float critMultiplierPercent => unitDataManager.CurrentUnitStatsData[EUnitFloatStats.CritMultiplierPercent];
    private float attackSpeedPercent => unitDataManager.CurrentUnitStatsData[EUnitFloatStats.AttackSpeedPercent];

    protected AIPathfinder AI;
    protected AATAgentController _agent;
    protected bool _canAttack;

    protected virtual void Awake()
    {
        _canAttack = true;
        AI = GetComponent<AIPathfinder>();
        AI.OnAttack += CallAttack;
        _agent = GetComponent<AATAgentController>();
    }

    public virtual void CallAttack(GameObject target)
    {
        transform.LookAt(target.transform);
        if (_agent.AgentEnabled)
            _agent.SetDestination(transform.position);
        if (!_canAttack) return;
        CheckCrit(target.GetComponent<UnitController>());
        StartCoroutine(StartAttackTimer());
    }

    protected IEnumerator StartAttackTimer()
    {
        _canAttack = false;
        yield return new WaitForSeconds(100 / attackSpeedPercent);
        _canAttack = true;
    }

    protected virtual void CheckCrit(UnitController target)
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

    protected virtual void BaseAttack(UnitController target)
    {
        target.GetComponent<IHealth>().ModifyHealth(-Mathf.Abs(damage));
    }

    protected virtual void CritAttack(UnitController target)
    {
        target.GetComponent<IHealth>().ModifyHealth(-Mathf.Abs(damage) * (critMultiplierPercent / 100));
    }
}

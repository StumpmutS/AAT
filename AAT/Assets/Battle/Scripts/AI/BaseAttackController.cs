using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathfinder))]
public class BaseAttackController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] protected UnitData unitData;
    
    private float Damage => unitData.Damage;
    private float damage;
    private float attackSpeedPercent => unitData.AttackSpeedPercent;
    private AIPathfinder AI;
    private bool canAttack;

    private void Awake()
    {
        damage = Damage;
    }

    private void Start()
    {
        canAttack = true;
        AI = GetComponent<AIPathfinder>();
        AI.OnAttack += CallAttack;
        damage = -(Mathf.Abs(damage));
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
        target.GetComponent<IHealth>().ModifyHealth(damage);
    }
}

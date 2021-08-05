using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathfinder), typeof(NavMeshAgent))]
public class BaseAttackController : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float attackTimerSeconds;

    private NavMeshAgent agent;
    private AIPathfinder AI;
    private bool canAttack;
    
    private void Start()
    {
        canAttack = true;
        AI = GetComponent<AIPathfinder>();
        agent = GetComponent<NavMeshAgent>();
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
        yield return new WaitForSeconds(attackTimerSeconds);
        canAttack = true;
    }

    protected virtual void Attack(GameObject target)
    {
        transform.LookAt(target.transform);
        agent.SetDestination(transform.position);
    }
}

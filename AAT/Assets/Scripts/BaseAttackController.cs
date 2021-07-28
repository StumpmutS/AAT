using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathfinder))]
public class BaseAttackController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float damage;
    [SerializeField] private float attackTimerSeconds;
    [SerializeField] GameObject acorn;
    [SerializeField] Vector3 offset;

    private AIPathfinder AI;
    private bool canAttack;
    
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
        yield return new WaitForSeconds(attackTimerSeconds);
        canAttack = true;
    }

    protected virtual void Attack(GameObject target)
    {
        Instantiate(acorn, transform.position - offset, Quaternion.identity);
        transform.LookAt(target.transform);
        agent.SetDestination(transform.position);
        GetComponent<Animator>().SetBool("Attack", true);
    }
}

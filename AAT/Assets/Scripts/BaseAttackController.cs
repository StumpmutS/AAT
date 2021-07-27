using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathfinder))]
public class BaseAttackController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float damage;

    private AIPathfinder AI;

    private void Start()
    {
        AI = GetComponent<AIPathfinder>();
        AI.OnAttack += Attack;
        damage = -(Mathf.Abs(damage));
    }

    protected virtual void Attack(GameObject target)
    {
        agent.SetDestination(transform.position);
        target.GetComponent<IHealth>().ModifyHealth(damage);
    }
}

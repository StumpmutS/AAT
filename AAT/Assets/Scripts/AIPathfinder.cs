using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathfinder : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    private void Start()
    {
        agent.SetDestination(new Vector3(transform.position.x + 5, transform.position.y, transform.position.z));
    }
}

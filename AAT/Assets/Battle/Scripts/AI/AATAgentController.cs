using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AATAgentController : MonoBehaviour
{
    private NavMeshAgent _agent;
    //HASHSET DOES NOT ALLOW DUPLICATES, THIS IS SAFE
    private HashSet<MonoBehaviour> _disablers = new HashSet<MonoBehaviour>();

    public Vector3 DesiredDestination { get; private set; }
    public bool AgentEnabled => _agent.enabled;
    public float RemainingDistance => _agent.remainingDistance;
    public float Speed => _agent.speed;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 destination)
    {
        DesiredDestination = destination;
        _agent.SetDestination(destination);
    }

    public void SetSpeed(float speed)
    {
        _agent.speed = speed;
    }

    public void EnableAgent(MonoBehaviour caller)
    {
        _disablers.Remove(caller);
        if (_disablers.Count <= 0) _agent.enabled = true;
    }

    public void DisableAgent(MonoBehaviour caller)
    {
        _disablers.Add(caller);
        _agent.enabled = false;
    }

    public void Warp(Vector3 desiredPos)
    {
        _agent.Warp(desiredPos);
    }
}

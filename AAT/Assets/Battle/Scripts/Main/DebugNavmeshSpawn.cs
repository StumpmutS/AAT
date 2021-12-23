using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AATAgentController))]
public class DebugNavmeshSpawn : MonoBehaviour
{
    AATAgentController agent;
    void Start()
    {
        agent = GetComponent<AATAgentController>();
        agent.Warp(transform.position);
        agent.SetDestination(transform.position + (Vector3.forward * .01f));
    }
}

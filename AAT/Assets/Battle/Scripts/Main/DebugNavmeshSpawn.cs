using UnityEngine;

public class DebugNavmeshSpawn : MonoBehaviour
{
    private void Start()
    {
        var agent = GetComponent<AgentBrain>().CurrentAgent;
        agent.Warp(transform.position);
        agent.SetDestination(transform.position + (Vector3.forward * .01f));
    }
}

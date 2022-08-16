using UnityEngine;

public class DebugNavmeshSpawn : MonoBehaviour
{
    private void Start()
    {
        if (!StumpNetworkRunner.Instance.Runner.IsServer) return;
        
        var agent = GetComponent<AgentBrain>().CurrentAgent;
        agent.Warp(transform.position);
        agent.SetDestination(transform.position + (Vector3.forward * .01f));
    }
}

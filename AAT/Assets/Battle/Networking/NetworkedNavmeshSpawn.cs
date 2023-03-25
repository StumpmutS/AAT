using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class NetworkedNavmeshSpawn : SimulationBehaviour, ISpawned
{
    [SerializeField] private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent.enabled = false;
    }

    public void Spawned()
    {
        navMeshAgent.enabled = true;
    }
}

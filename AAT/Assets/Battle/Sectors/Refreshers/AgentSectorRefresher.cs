using Fusion;
using UnityEngine;

public class AgentSectorRefresher : SimulationBehaviour, ISpawned
{
    [SerializeField] private SectorReference sectorReference;
    [SerializeField] private AgentBrain agentBrain;


    public void Spawned()
    {
        agentBrain.OnWarped += HandleWarped;
    }

    private void HandleWarped()
    {
        sectorReference.RefreshSector();
    }
}
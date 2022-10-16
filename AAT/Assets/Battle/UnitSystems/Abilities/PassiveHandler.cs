using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PassiveHandler : SimulationBehaviour, ISpawned, IDespawned
{
    [SerializeField] UnitController unit;
    [SerializeField] private List<UnitPassiveData> unitPassiveData;

    public void Spawned()
    {
        if (!Runner.IsServer) return;
        
        unitPassiveData.ForEach(u => u.UnitPassiveDataInfo.PassiveComponents.ForEach(c => c.ActivateComponent(unit)));
    }

    public void Despawned(NetworkRunner runner, bool hasState)
    {
        if (!Runner.IsServer) return;

        unitPassiveData.ForEach(u => u.UnitPassiveDataInfo.PassiveComponents.ForEach(c => c.DeactivateComponent(unit)));
    }
}

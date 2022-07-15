using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class SectorController : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnChange))]
    public int SectorPower { get; set; }

    public event Action<SectorController> OnSectorPowerChanged = delegate { };

    public static void OnChange(Changed<SectorController> changed)
    {
        changed.Behaviour.OnSectorPowerChanged.Invoke(changed.Behaviour);
    }

    public void ModifySectorPower(int amount)
    {
        SectorPower += amount;
    }
}

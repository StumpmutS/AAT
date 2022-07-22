using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [Networked, Capacity(128)] public NetworkLinkedList<SectorController> OwnedSectors => default;

    public void AddSectors(IEnumerable<SectorController> sectors)
    {
        foreach (var sector in sectors)
        {
            OwnedSectors.Add(sector);
        }
    }
}

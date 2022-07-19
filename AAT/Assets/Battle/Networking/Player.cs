using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public List<SectorController> OwnedSectors { get; private set; } = new();

    public void AddSectors(IEnumerable<SectorController> sectors)
    {
        OwnedSectors.AddRange(sectors);
    }
}

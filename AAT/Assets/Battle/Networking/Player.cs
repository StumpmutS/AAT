using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public List<SectorController> OwnedSectors { get; private set; }

    public void Init(List<SectorController> sectors)
    {
        OwnedSectors = sectors;
        
    }
}

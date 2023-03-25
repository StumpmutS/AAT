using Fusion;
using UnityEngine;

public struct NetworkedStumpTarget : INetworkStruct
{
    public NetworkId Id;
    public Vector3 Point;
    
    public NetworkedStumpTarget(NetworkId id, Vector3 point)
    {
        Id = id;
        Point = point;
    }
}
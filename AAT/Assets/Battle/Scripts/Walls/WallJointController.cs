using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJointController : MonoBehaviour
{
    private List<PlaceableWallController> _connectedWalls = new List<PlaceableWallController>();
    public List<PlaceableWallController> ConnectedWalls => _connectedWalls;

    public void AddWall(PlaceableWallController wall)
    {
        _connectedWalls.Add(wall);
    }

    public PlaceableWallController OtherWall(PlaceableWallController fromWall)
    {
        foreach (var wall in _connectedWalls)
        {
            if (wall != fromWall) return wall;
        }
        return null;
    }
}

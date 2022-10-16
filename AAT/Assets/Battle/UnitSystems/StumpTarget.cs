using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StumpTarget
{
    public GameObject Hit { get; private set; }
    public Vector3 Point;

    public StumpTarget(GameObject hit, Vector3 point)
    {
        this.Hit = hit;
        this.Point = point;
    }
}


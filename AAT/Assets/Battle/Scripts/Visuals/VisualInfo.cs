using System;
using UnityEngine;

[Serializable]
public struct VisualInfo
{
    public Color Color;
    public int Severity;
    public Vector3 Direction;
    public float DirectionMultiplier;
    public Vector3 Position;
    public Quaternion Rotation;
    public float Randomize;

    public VisualInfo(Color color, int severity, Vector3 direction, float directionMultiplier, Vector3 position, Quaternion rotation, float randomize = 0)
    {
        Color = color;
        Severity = severity;
        Direction = direction;
        DirectionMultiplier = directionMultiplier;
        Position = position;
        Rotation = rotation;
        Randomize = randomize;
    }
}
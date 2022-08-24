using UnityEngine;

public struct AttackDecalInfo
{
    public Color Color;
    public int Severity;
    public Vector3 Direction;
    public Vector3 Position;
    public float Randomize;

    public AttackDecalInfo(Color color, int severity, Vector3 direction, Vector3 position, float randomize = 0)
    {
        Color = color;
        Severity = severity;
        Direction = direction;
        Position = position;
        Randomize = randomize;
    }
}
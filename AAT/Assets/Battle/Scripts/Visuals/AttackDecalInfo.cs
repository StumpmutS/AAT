using UnityEngine;

public struct AttackDecalInfo
{
    public Color Color;
    public int Severity;
    public Vector3 Direction;
    public Vector3 Position;

    public AttackDecalInfo(Color color, int severity, Vector3 direction, Vector3 position)
    {
        Color = color;
        Severity = severity;
        Direction = direction;
        Position = position;
    }
}
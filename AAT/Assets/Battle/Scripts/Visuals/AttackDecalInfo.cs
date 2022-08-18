using UnityEngine;

public struct AttackDecalInfo
{
    public Color Color;
    public int Severity;
    public Vector3 Direction;

    public AttackDecalInfo(Color color, int severity, Vector3 direction)
    {
        Color = color;
        Severity = severity;
        Direction = direction;
    }
}
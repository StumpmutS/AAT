using System;
using UnityEngine;

[Serializable]
public class RectStretchValues
{
    [SerializeField] private Vector2 leftBottom;
    public Vector2 LeftBottom => leftBottom;
    [SerializeField] private Vector2 rightTop;
    public Vector2 RightTop => rightTop;

    public static RectStretchValues operator *(RectStretchValues a, float b)
    {
        return new RectStretchValues(a.LeftBottom * b, a.RightTop * b);
    }

    public static RectStretchValues operator +(RectStretchValues a, RectStretchValues b)
    {
        return new RectStretchValues(a.LeftBottom + b.LeftBottom, a.RightTop + b.RightTop);
    }

    public static RectStretchValues operator -(RectStretchValues a, RectStretchValues b)
    {
        return new RectStretchValues(a.LeftBottom - b.LeftBottom, a.RightTop - b.RightTop);
    }

    public RectStretchValues(Vector2 leftBottom, Vector2 rightTop)
    {
        this.leftBottom = leftBottom;
        this.rightTop = rightTop;
    }
}
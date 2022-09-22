using System;
using UnityEngine;

[Serializable]
public class StylizedText
{
    [SerializeField] private string text;
    public string Text => text;
    [SerializeField] private Color textColor;
    public Color TextColor => textColor;
    [SerializeField] private float fontSize;
    public float FontSize => fontSize;
    [SerializeField] private RectStretchValues textOffsets;
    public RectStretchValues TextOffsets => textOffsets;
}
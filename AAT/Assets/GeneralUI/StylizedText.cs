using System;
using UnityEngine;

[Serializable]
public class StylizedText
{
    public string Text;
    public Color TextColor;
    public float FontSize;
    public RectStretchValues TextOffsets;

    public StylizedText(string text, Color textColor, float fontSize, RectStretchValues textOffsets)
    {
        Text = text;
        TextColor = textColor;
        FontSize = fontSize;
        TextOffsets = textOffsets;
    }
}

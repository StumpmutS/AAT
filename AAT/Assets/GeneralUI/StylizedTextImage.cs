using System;
using UnityEngine;

[Serializable]
public class StylizedTextImage
{
    [SerializeField] private StylizedImage image;
    public StylizedImage Image => image;
    [SerializeField] private StylizedText text;
    public StylizedText Text => text;
}
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StylizedImage
{
    [SerializeField] private List<Sprite> fills;
    public List<Sprite> Fills => fills;
    [SerializeField] private List<Color> fillColors;
    public List<Color> FillColors => fillColors;
    
    [SerializeField] private Sprite lighting;
    public Sprite Lighting => lighting;
    [SerializeField] private Color lightingColor;
    public Color LightingColor => lightingColor;
    
    [SerializeField] private Sprite shading;
    public Sprite Shading => shading;
    [SerializeField] private Color shadingColor;
    public Color ShadingColor => shadingColor;
    
    [SerializeField] private Sprite border;
    public Sprite Border => border;
    [SerializeField] private Color borderColor;
    public Color BorderColor => borderColor;
    [SerializeField] private RectStretchValues borderOffsets;
    public RectStretchValues BorderOffsets => borderOffsets;
}
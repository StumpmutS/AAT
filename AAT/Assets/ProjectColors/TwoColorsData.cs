using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Colors/Two Colors")]
public class TwoColorsData : ScriptableObject
{
    [SerializeField] private Color color1;
    public Color Color1 => color1;
    [SerializeField] private Color color2;
    public Color Color2 => color2;
}

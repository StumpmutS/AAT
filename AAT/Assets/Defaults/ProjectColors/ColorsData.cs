using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectDefaults/Colors List")]
public class ColorsData : ScriptableObject
{
    [SerializeField] private List<Color> colors;
    public List<Color> Colors => colors;
}

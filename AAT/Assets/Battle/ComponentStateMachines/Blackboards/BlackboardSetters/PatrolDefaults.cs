using System.Collections.Generic;
using UnityEngine;

public class PatrolDefaults : ScriptableObject
{
    [SerializeField] private List<StylizedTextImage> icon;
    public List<StylizedTextImage> Icon => icon;
    [SerializeField] private string label;
    public string Label => label;
    [SerializeField] private KeyCode keyCode;
    public KeyCode KeyCode => keyCode;
    [SerializeField] private ESelectionType category;
    public ESelectionType Category => category;
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class UserAction
{
    public string Category { get; }
    public ESubCategory SubCategory { get; }
    public string Label { get; }
    public Action<object> Action { get; }
    public object ActionObj { get; }
    public List<StylizedTextImage> IconSet { get; }
    public KeyCode KeyCode { get; }

    public UserAction(string category, ESubCategory subCategory, string label, List<StylizedTextImage> icon, Action<object> action, object actionObj, KeyCode keyCode)
    {
        Category = category; // ex: Wolf Unit
        SubCategory = subCategory; // ex: ESubCategory.Ability
        Label = label; // ex: Howl
        IconSet = icon;
        Action = action;
        ActionObj = actionObj;
        KeyCode = keyCode;
    }
}

[Serializable]
public class IconSet
{
    public List<StylizedTextImage> SelectedIcon { get; }
    public List<StylizedTextImage> DeselectedIcon { get; }
    public List<StylizedTextImage> MixedIcon { get; }
}

public enum ESubCategory
{
    None,
    Ability,
    Movement, // all but ability are tentative
    Spawning,
    Upgrades
}
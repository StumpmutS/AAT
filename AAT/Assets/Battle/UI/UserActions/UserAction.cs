using System;
using System.Collections.Generic;
using UnityEngine;

public class UserAction
{
    private readonly IActionCreator _actionCreator;
    public string Category { get; }
    public ESubCategory SubCategory { get; }
    public string Label { get; }
    public Action<object> SelectedAction { get; }
    public Action<object> DeselectedAction { get; }
    public object ActionObj { get; }
    public List<StylizedTextImage> Icon { get; }
    public KeyCode KeyCode { get; }

    public UserAction(IActionCreator actionCreator, string category, ESubCategory subCategory, string label, List<StylizedTextImage> icon, Action<object> selectedAction, Action<object> deselectedAction, object actionObj, KeyCode keyCode)
    {
        _actionCreator = actionCreator;
        Category = category; // ex: Wolf Group
        SubCategory = subCategory; // ex: ESubCategory.Ability
        Label = label; // ex: Howl
        Icon = icon;
        SelectedAction = selectedAction;
        DeselectedAction = deselectedAction;
        ActionObj = actionObj;
        KeyCode = keyCode;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        var otherAction = (UserAction) obj;
        return (otherAction._actionCreator == this._actionCreator && otherAction.Category == this.Category &&
                (int) otherAction.SubCategory == (int) this.SubCategory && otherAction.Label == this.Label);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_actionCreator, Category, (int) SubCategory, Label);
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
    Movement,
    Spawning,
    Upgrades
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIfAttribute : PropertyAttribute
{
    public string Condition;
    public bool Value;
    public int HeightMultiplier;

    public ShowIfAttribute(string condition, bool value, int heightMultiplier = 1)
    {
        Condition = condition;
        Value = value;
        HeightMultiplier = heightMultiplier;
    }
}

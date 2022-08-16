using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIfAttribute : PropertyAttribute
{
    public string Condition;
    public bool Value;

    public ShowIfAttribute(string condition, bool value)
    {
        Condition = condition;
        Value = value;
    }
}

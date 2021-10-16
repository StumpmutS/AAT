using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoolButtonController : UnitDeterminedUI
{
    [SerializeField] private bool startValue;
    [SerializeField] private Color trueColor;
    [SerializeField] private Color falseColor;
    [SerializeField] private List<Image> imagesToAlter;

    private bool selected;

    public UnityEvent<bool> OnBoolChanged;

    private void Start()
    {
        selected = !startValue;
        SetBoolElement();
    }

    public void SetBoolElement()
    {
        selected = !selected;
        OnBoolChanged.Invoke(selected);
        SetImages(selected);
    }

    public override void SetToUnitPreference(bool preference)
    {
        if (preference == selected) return;
        selected = preference;
        SetImages(preference);
    }

    private void SetImages(bool selected)
    {
        foreach (var image in imagesToAlter)
        {
            image.color = selected ? trueColor : falseColor;
        }
    }
}

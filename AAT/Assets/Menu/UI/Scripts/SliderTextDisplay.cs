using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextDisplay : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleValueChanged);
    }

    private void Start()
    {
        SetTextToValue(slider.value);
    }

    private void HandleValueChanged(float value)
    {
        SetTextToValue(value);
    }

    private void SetTextToValue(float value)
    {
        text.text = slider.wholeNumbers ? Mathf.RoundToInt(value).ToString() : value.ToString("F2");
    }

    private void OnDestroy()
    {
        if (slider != null) slider.onValueChanged.RemoveListener(HandleValueChanged);
    }
}

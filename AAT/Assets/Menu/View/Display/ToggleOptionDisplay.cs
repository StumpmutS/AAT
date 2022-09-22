using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOptionDisplay : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private TMP_Text textTarget;
    [SerializeField] private string option1, option2;
    
    private void Awake()
    {
        toggle.onValueChanged.AddListener(HandleToggle);
    }

    private void Start()
    {
        HandleToggle(toggle.isOn);
    }

    private void HandleToggle(bool value)
    {
        textTarget.text = value ? option1 : option2;
    }
}

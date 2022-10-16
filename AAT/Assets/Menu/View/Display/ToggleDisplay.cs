using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class ToggleDisplay : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [FormerlySerializedAs("tabSpring")] [SerializeField] private SpringController toggleSpring;
    [SerializeField] private float activeNudgeSpeed;
    [SerializeField] private bool useDivider;
    [SerializeField, ShowIf(nameof(useDivider), true)] private GameObject divider;

    private bool _active;
    
    private void Awake()
    {
        toggle.onValueChanged.AddListener(UpdateDisplay);
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        UpdateDisplay(toggle.isOn);
    }

    private void UpdateDisplay(bool value)
    {
        if (value)
        {
            ActivateVisuals();
        }
        else
        {
            DeactivateVisuals();
        }
    }

    private void ActivateVisuals()
    {
        _active = true;
        toggleSpring.SetTarget(1);
        if (useDivider) divider.SetActive(true);
    }

    private void DeactivateVisuals()
    {
        _active = false;
        toggleSpring.SetTarget(-1);
        if (useDivider) divider.SetActive(false);
    }

    public void HandlePointerDown()
    {
        if (_active) toggleSpring.Nudge(activeNudgeSpeed);
    }

    public void HandlePointerEnter()
    {
        if (!_active) toggleSpring.SetTarget(0);
        else toggleSpring.Nudge(activeNudgeSpeed);
    }

    public void HandlePointerExit()
    {
        if (!_active) toggleSpring.SetTarget(-1);
    }
}
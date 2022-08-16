using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonInteractionSpringController : MonoBehaviour
{
    [SerializeField] private SpringController interactionSpring;

    public void OnPointerEnter(BaseEventData _)
    {
        interactionSpring.SetTarget(1);
    }

    public void OnPointerDown(BaseEventData _)
    {
        interactionSpring.SetTarget(-1);
    }

    public void OnPointerUp(BaseEventData _)
    {
        interactionSpring.SetTarget(0);
    }

    public void OnPointerExit(BaseEventData _)
    {
        interactionSpring.SetTarget(0);
    }
}

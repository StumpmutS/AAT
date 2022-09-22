using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class InteractionSpringDisplay : MonoBehaviour
{
    [SerializeField] private SpringController interactionSpring;
    [SerializeField] private int onDefault, onEnter, onDown;

    public void OnPointerEnter(BaseEventData _)
    {
        interactionSpring.SetTarget(onEnter);
    }

    public void OnPointerDown(BaseEventData _)
    {
        interactionSpring.SetTarget(onDown);
    }

    public void OnPointerUp(BaseEventData _)
    {
        interactionSpring.SetTarget(onDefault);
    }

    public void OnPointerExit(BaseEventData _)
    {
        interactionSpring.SetTarget(onDefault);
    }
}

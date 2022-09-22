using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

public abstract class VisualsSender : MonoBehaviour
{
    [SerializeField] protected Fusion.SerializableDictionary<VisualComponent, VisualInfo> visuals;

    protected void SendVisuals(GameObject toGameObj, Dictionary<VisualComponent, VisualInfo> sendData)
    {
        var visualListener = toGameObj.AddOrGetComponent<VisualsListener>();
        visualListener.AcceptVisuals(sendData);
    }
}
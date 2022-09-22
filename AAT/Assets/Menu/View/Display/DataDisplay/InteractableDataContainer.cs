using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableDataContainer : MonoBehaviour
{
    public abstract void DisplayData(List<StumpData> data, UnityAction<int, StumpData> callback);
    public abstract void RemoveDisplay();
}
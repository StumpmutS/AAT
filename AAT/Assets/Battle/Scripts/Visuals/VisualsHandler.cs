using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualsHandler : MonoBehaviour
{
    private UnitController _unit;

    private void Awake()
    {
        _unit = GetComponent<UnitController>();
    }

    public void ActivateVisuals(IEnumerable<VisualComponent> visualComponents)
    {
        foreach (var component in visualComponents)
        {
            StartCoroutine(CoActivateVisualComponent(component));
        }
    }

    private IEnumerator CoActivateVisualComponent(VisualComponent visualComponent)
    {
        yield return new WaitForSeconds(visualComponent.ComponentDelay);
        visualComponent.ActivateComponent(_unit);
        yield return new WaitForSeconds(visualComponent.ComponentDuration);
        visualComponent.DeactivateComponent(_unit);
    }
}

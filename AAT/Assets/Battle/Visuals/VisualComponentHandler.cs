using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VisualComponentHandler : MonoBehaviour
{
    private UnitController _unit;

    private void Awake()
    {
        _unit = GetComponent<UnitController>();
    }

    public void ActivateVisuals(IEnumerable<UnitVisualComponent> visualComponents)
    {
        foreach (var component in visualComponents)
        {
            StartCoroutine(CoActivateVisualComponent(component));
        }
    }

    private IEnumerator CoActivateVisualComponent(UnitVisualComponent unitVisualComponent)
    {
        yield return new WaitForSeconds(unitVisualComponent.Delay);
        unitVisualComponent.ActivateComponent(_unit);
        yield return new WaitForSeconds(unitVisualComponent.Duration);
        unitVisualComponent.DeactivateComponent(_unit);
    }
}

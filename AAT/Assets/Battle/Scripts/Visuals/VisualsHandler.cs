using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualsHandler : MonoBehaviour
{
    [SerializeField] private DecalImage attackDecal;
    
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
        yield return new WaitForSeconds(visualComponent.Delay);
        visualComponent.ActivateComponent(_unit);
        yield return new WaitForSeconds(visualComponent.Duration);
        visualComponent.DeactivateComponent(_unit);
    }

    public void CreateDecal(DecalImage image, AttackDecalInfo info)
    {
        var decal = Instantiate(image, transform.position, Quaternion.identity);
        decal.Activate(info);
    }
}

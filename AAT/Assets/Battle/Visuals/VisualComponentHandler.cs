using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualComponentHandler : MonoBehaviour
{
    public void ActivateVisuals(IEnumerable<VisualComponent> visualComponents, VisualInfo info)
    {
        foreach (var component in visualComponents)
        {
            StartCoroutine(CoActivateVisualComponent(component, info));
        }
    }

    private IEnumerator CoActivateVisualComponent(VisualComponent visualGameAction, VisualInfo info)
    {
        yield return new WaitForSeconds(visualGameAction.Delay);
        visualGameAction.ActivateComponent(info);
        yield return new WaitForSeconds(visualGameAction.Duration);
        visualGameAction.DeactivateComponent(info);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VisualsHandler : MonoBehaviour
{
    [SerializeField] private float decalDirectionMultiplier = 2;
    [SerializeField] private Vector3 decalOffset;
    
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
        var position = info.Position - info.Direction.normalized * decalDirectionMultiplier + decalOffset;
        position += Vector3.one * Random.Range(0, info.Randomize);
        var decal = PoolingManager.Instance.CreatePoolingObject(image.PoolingObj);
        decal.transform.position = position;
        decal.GetComponent<DecalImage>().Activate(info);
    }
}

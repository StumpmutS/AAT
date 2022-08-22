using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecalImage : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private ColorSpringListener colorSpring;
    [SerializeField] private List<DecalComponent> decalComponents;
    [SerializeField] private List<Image> images;
    public int MaxSeverity => images.Count;

    public void Activate(AttackDecalInfo info)
    {
        transform.position += offset;
        DeactivateImages();

        colorSpring.SetMaxColor(info.Color);

        ActivateImages(info.Severity);
    }

    private void DeactivateImages()
    {
        foreach (var image in images)
        {
            image.gameObject.SetActive(false);
        }
        
        foreach (var decal in decalComponents)
        {
            decal.Deactivate();
        }
    }

    private void ActivateImages(int severity)
    {
        for (int i = 0; i < severity; i++)
        { 
            images[i].gameObject.SetActive(true);
        }
        
        foreach (var decalComponent in decalComponents)
        {
            StartCoroutine(CoAnimateDecalComponent(decalComponent));
        }
    }

    private IEnumerator CoAnimateDecalComponent(DecalComponent component)
    {
        yield return new WaitForSeconds(component.Delay);
        component.Activate();
        yield return new WaitForSeconds(component.Duration);
        component.Deactivate();
    }
}
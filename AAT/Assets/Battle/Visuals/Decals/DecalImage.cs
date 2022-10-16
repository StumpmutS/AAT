using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.Scripts;

[RequireComponent(typeof(PoolingObject))]
public class DecalImage : MonoBehaviour
{
    [SerializeField] private PoolingObject poolObj;
    public PoolingObject PoolObj => poolObj;
    [SerializeField] private ColorSpringListener colorSpring;
    [SerializeField] private List<Image> images;

    public void Activate(VisualInfo info)
    {
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
    }

    private void ActivateImages(int severity)
    {
        for (int i = 0; i < severity; i++)
        { 
            images[i].gameObject.SetActive(true);
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(MainCameraRef.Cam.transform.position);
    }
}
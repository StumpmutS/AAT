using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityPreviewPoolingObject : BasePreviewPoolingObject
{
    [SerializeField] private GameObject actualPrefab;
    
    protected override GameObject CreateObject()
    {
        return Instantiate(actualPrefab, transform.position, transform.rotation);
    }
}

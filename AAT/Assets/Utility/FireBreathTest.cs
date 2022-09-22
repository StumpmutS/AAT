using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class FireBreathTest : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private VisualEffect column;
    [SerializeField] private VisualEffect target;
    
    [SerializeField] private float coneRadius;
    
    public void Update()
    {
        Physics.Raycast(head.position, head.forward, out var hit, 100, ~0);
        var range = Vector3.Distance(head.position, hit.point);
    }
}

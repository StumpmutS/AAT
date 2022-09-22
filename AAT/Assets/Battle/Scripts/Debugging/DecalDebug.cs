using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DecalDebug : MonoBehaviour
{
    [SerializeField] private DecalImage decal;
    [SerializeField] private Color color;
    [SerializeField] private int severity;
    [SerializeField] private Vector3 direction;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(decal).Activate(new VisualInfo(color, severity, direction, 0, BaseInputManager.LeftClickPosition, Quaternion.identity));
        }
    }
}

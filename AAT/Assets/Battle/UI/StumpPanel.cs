using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StumpPanel : MonoBehaviour
{
    [SerializeField] private SpringController spring;
    
    public void Activate(GameObject go)
    {
        if (spring != null) spring.SetTarget(0);
    }

    public void Deactivate()
    {
        if (spring != null) spring.SetTarget(1);
    }
}

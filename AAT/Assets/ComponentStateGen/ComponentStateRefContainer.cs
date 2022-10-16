using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentStateRefContainer : MonoBehaviour
{
    [SerializeField] private ComponentStatesRef componentStatesRef;
    public ComponentStatesRef ComponentStatesRef => componentStatesRef;
    
    public static ComponentStateRefContainer Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}

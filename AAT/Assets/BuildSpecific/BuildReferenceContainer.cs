using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildReferenceContainer : MonoBehaviour
{
    [SerializeField] private BuildResourceReference buildResourceReference;
    public BuildResourceReference BuildResourceReference => buildResourceReference;

    public static BuildReferenceContainer Instance;
    
    private void Awake() => Instance = this;
}

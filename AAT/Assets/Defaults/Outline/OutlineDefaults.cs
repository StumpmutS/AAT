using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "ProjectDefaults/OutlineInfo")]
public class OutlineDefaults : ScriptableObject
{
    [SerializeField] private CompareFunction maskZTest;
    public CompareFunction MaskZTest => maskZTest;
}

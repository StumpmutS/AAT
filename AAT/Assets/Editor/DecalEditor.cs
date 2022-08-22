using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DecalImage))]
public class DecalEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var decal = (DecalImage) target;
        if (GUILayout.Button("Test Decal Animation"))
        {
            decal.Activate(new AttackDecalInfo(Color.blue, Mathf.CeilToInt(decal.MaxSeverity / 2f), Vector3.forward, Vector3.zero));
        }
        if (GUILayout.Button("Deactivate Decal Animation"))
        {
            decal.Activate(new AttackDecalInfo(Color.white, decal.MaxSeverity, -Vector3.forward, Vector3.zero));
        }
    }
}

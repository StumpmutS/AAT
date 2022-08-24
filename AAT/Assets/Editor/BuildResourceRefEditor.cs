using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildResourceReference))]
public class BuildResourceRefEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var buildRef = (BuildResourceReference) target;
        if (GUILayout.Button("Reimport"))
        {
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(buildRef));
        }
    }
}

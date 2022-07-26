using UnityEditor;
using UnityEngine;

namespace Utility.Scripts
{
    public static class ResourceHelper
    {
        public static string GetResourcePath(this ScriptableObject so)
        {
            var assetPath = AssetDatabase.GetAssetPath(so); //remove ...Resources/ - .asset
            var noExtPath = assetPath.Remove(assetPath.IndexOf(".asset"), 6);
            return noExtPath.Remove(0, assetPath.IndexOf("Resources/") + 10);
        }
    }
}

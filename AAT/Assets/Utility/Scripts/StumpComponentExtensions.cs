using UnityEngine;

namespace Utility.Scripts
{
    public static class StumpComponentExtensions
    {
        public static T AddOrGetComponent<T>(this Component component) where T : Component
        {
            if (!component.TryGetComponent<T>(out var found))
            {
                found = component.gameObject.AddComponent<T>();
            }

            return found;
        }
        
        public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out var found))
            {
                found = gameObject.gameObject.AddComponent<T>();
            }

            return found;
        }
    }
}
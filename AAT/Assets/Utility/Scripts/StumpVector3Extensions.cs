using UnityEngine;

namespace Utility.Scripts
{
    public static class StumpVector3Extensions
    {
        public static bool SetToCursorToWorldPosition(this ref Vector3 vector3)
        {
            var ray = MainCameraRef.Cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return false;
            vector3 = hit.point;
            return true;
        }
        public static bool SetToCursorToWorldPosition(this ref Vector3 vector3, LayerMask layer)
        {
            var ray = MainCameraRef.Cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, layer)) return false;
            vector3 = hit.point;
            return true;
        }
    }
}

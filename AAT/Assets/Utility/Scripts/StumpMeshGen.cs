using UnityEngine;

namespace Utility.Scripts
{
    public static class StumpMeshGen
    {
        public static void GenerateQuadData(Vector3 point1, Vector3 point2, float width, out Vector3[] vertices, out int[] tris, out Vector3[] normals, out Vector2[] texCoords)
        {
            var forwardDir = point2 - point1;
            var sideDir = Vector3.Cross(Vector3.up, forwardDir).normalized;
        
            vertices = new[]
            {
                point1 - sideDir * width,
                point1 + sideDir * width,
                point2 - sideDir * width,
                point2 + sideDir * width
            };
        
            tris = new[]
            {
                0, 1, 2,
                3, 2, 1
            };

            normals = new []
            {
                Vector3.Cross(-sideDir, forwardDir),
                Vector3.Cross(sideDir, forwardDir),
                Vector3.Cross(-sideDir, forwardDir),
                Vector3.Cross(sideDir, forwardDir)
            };

            texCoords = new[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
        }
    }
}
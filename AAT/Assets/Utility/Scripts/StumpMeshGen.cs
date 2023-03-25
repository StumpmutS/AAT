using UnityEngine;

namespace Utility.Scripts
{
    public static class StumpMeshGen
    {
        public static void GenerateQuadData(Vector3 startMidPoint, Vector3 endMidPoint, float width, out Vector3[] vertices, out int[] tris, out Vector3[] normals, out Vector2[] texCoords)
        {
            var forwardDir = endMidPoint - startMidPoint;
            var sideDir = Vector3.Cross(Vector3.up, forwardDir).normalized;
        
            vertices = new[]
            {
                startMidPoint - sideDir * width,
                startMidPoint + sideDir * width,
                endMidPoint - sideDir * width,
                endMidPoint + sideDir * width
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

        public static int[] HexahedronTris = {
            //top
            0, 1, 2,
            3, 2, 1,
            //sides
            0, 1, 4,
            5, 4, 1,
            1, 3, 5,
            7, 5, 3,
            3, 2, 7,
            6, 7, 2,
            2, 0, 6,
            4, 6, 0,
            //bottom
            4, 5, 6,
            7, 6, 5
        };
    }
}
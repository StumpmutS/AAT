using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

[ExecuteAlways]
public class StumpLineRenderer : MonoBehaviour
{
    [SerializeField] private float width;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] protected List<Vector3> points;

    private Mesh _mesh;
    private Mesh Mesh
    {
        get
        {
            if (_mesh == null)
            {
                if (meshFilter.sharedMesh == null)
                {
                    _mesh = new Mesh
                    {
                        name = "Line Mesh"
                    };
                    meshFilter.sharedMesh = _mesh;
                }
            }
            return _mesh;
        }
    }
    private bool _isDirty;

    protected virtual void Update()
    {
        if (!_isDirty) return;
        _isDirty = false;
        CreateLine(points);
    }

    private void CreateLine(List<Vector3> linePoints)
    {
        Mesh.Clear();

        List<Vector3> vertices = new();
        List<int> tris = new();
        List<Vector3> normals = new();
        List<Vector2> texCoords = new();

        for (int i = 0; i < linePoints.Count - 1; i++)
        {
            StumpMeshGen.GenerateQuadData(linePoints[i], linePoints[i + 1], width, out var quadVertices, out var quadTris, out var quadNormals, out var quadTexCoords);
            for (int j = 0; j < quadTris.Length; j++)
            {
                quadTris[j] += i * 4;
            }

            if (vertices.Count >= 2)
            {
                quadVertices[0] = vertices[^2];
                quadVertices[1] = vertices[^1];
            }

            var bottomCoords = (float) i / (linePoints.Count - 1);
            var topCoords = (float) (i + 1) / (linePoints.Count - 1);
            quadTexCoords = new[]
            {
                new Vector2(0, bottomCoords),
                new Vector2(1, bottomCoords),
                new Vector2(0, topCoords),
                new Vector2(1, topCoords)
            };

            vertices.AddRange(quadVertices);
            tris.AddRange(quadTris);
            normals.AddRange(quadNormals);
            texCoords.AddRange(quadTexCoords);
        }

        Mesh.vertices = vertices.ToArray();
        Mesh.triangles = tris.ToArray();
        Mesh.normals = normals.ToArray();
        Mesh.uv = texCoords.ToArray();
    }

    public void SetPoints(List<Vector3> newPoints)
    {
        _isDirty = true;
        points = newPoints;
    }
}
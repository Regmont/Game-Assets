using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ImprovedMeshToTerrainConverter : MonoBehaviour
{
    public MeshFilter sourceMeshFilter;
    public int terrainResolution = 512;
    public float terrainHeight = 100f;

    [ContextMenu("Convert Mesh To Terrain")]
    void ConvertMeshToTerrain()
    {
        if (sourceMeshFilter == null || sourceMeshFilter.sharedMesh == null)
        {
            Debug.LogError("No mesh assigned!");
            return;
        }

        Mesh mesh = sourceMeshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        Bounds bounds = mesh.bounds;
        Transform meshTransform = sourceMeshFilter.transform;

        float[,] heightMap = new float[terrainResolution, terrainResolution];

        // Spatial hash to speed up triangle lookup (optional future improvement)

        for (int x = 0; x < terrainResolution; x++)
        {
            for (int z = 0; z < terrainResolution; z++)
            {
                float percentX = x / (float)(terrainResolution - 1);
                float percentZ = z / (float)(terrainResolution - 1);

                Vector3 worldPos = new Vector3(
                    Mathf.Lerp(bounds.min.x, bounds.max.x, percentX),
                    0,
                    Mathf.Lerp(bounds.min.z, bounds.max.z, percentZ)
                );

                Vector3 localPoint = meshTransform.InverseTransformPoint(worldPos);
                float height = SampleHeightFromMesh(mesh, localPoint);
                heightMap[z, x] = Mathf.Clamp01(height / terrainHeight);
            }
        }

        // Create TerrainData
        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = terrainResolution;
        terrainData.size = bounds.size;
        terrainData.SetHeights(0, 0, heightMap);

        // Create terrain object
        GameObject terrainObj = Terrain.CreateTerrainGameObject(terrainData);
        terrainObj.transform.position = meshTransform.position;

        Debug.Log("Terrain created from mesh using triangle interpolation!");
    }

    float SampleHeightFromMesh(Mesh mesh, Vector3 localPoint)
    {
        Vector3[] verts = mesh.vertices;
        int[] tris = mesh.triangles;

        float closestHeight = 0f;
        float minDist = float.MaxValue;

        // For each triangle
        for (int i = 0; i < tris.Length; i += 3)
        {
            Vector3 v0 = verts[tris[i]];
            Vector3 v1 = verts[tris[i + 1]];
            Vector3 v2 = verts[tris[i + 2]];

            if (PointInTriangle(localPoint, v0, v1, v2))
            {
                Vector3 bary = Barycentric(localPoint, v0, v1, v2);
                float interpolatedHeight = v0.y * bary.x + v1.y * bary.y + v2.y * bary.z;
                return interpolatedHeight;
            }

            // Optional fallback: closest center point
            Vector3 center = (v0 + v1 + v2) / 3f;
            float dist = Vector2.SqrMagnitude(new Vector2(localPoint.x, localPoint.z) - new Vector2(center.x, center.z));
            if (dist < minDist)
            {
                minDist = dist;
                closestHeight = center.y;
            }
        }

        return closestHeight; // fallback if no triangle matched
    }

    bool PointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        Vector2 p2 = new Vector2(p.x, p.z);
        Vector2 a2 = new Vector2(a.x, a.z);
        Vector2 b2 = new Vector2(b.x, b.z);
        Vector2 c2 = new Vector2(c.x, c.z);

        float area = 0.5f * (-b2.y * c2.x + a2.y * (-b2.x + c2.x) + a2.x * (b2.y - c2.y) + b2.x * c2.y);
        float sign = area < 0 ? -1f : 1f;
        float s = (a2.y * c2.x - a2.x * c2.y + (c2.y - a2.y) * p2.x + (a2.x - c2.x) * p2.y) * sign;
        float t = (a2.x * b2.y - a2.y * b2.x + (a2.y - b2.y) * p2.x + (b2.x - a2.x) * p2.y) * sign;

        return s >= 0 && t >= 0 && (s + t) <= 2f * area * sign;
    }
    Vector3 Barycentric(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        Vector2 p2 = new Vector2(p.x, p.z);
        Vector2 a2 = new Vector2(a.x, a.z);
        Vector2 b2 = new Vector2(b.x, b.z);
        Vector2 c2 = new Vector2(c.x, c.z);

        Vector2 v0 = b2 - a2;
        Vector2 v1 = c2 - a2;
        Vector2 v2 = p2 - a2;

        float d00 = Vector2.Dot(v0, v0);
        float d01 = Vector2.Dot(v0, v1);
        float d11 = Vector2.Dot(v1, v1);
        float d20 = Vector2.Dot(v2, v0);
        float d21 = Vector2.Dot(v2, v1);
        float denom = d00 * d11 - d01 * d01;

        if (Mathf.Approximately(denom, 0f)) return new Vector3(1, 0, 0); // fallback

        float v = (d11 * d20 - d01 * d21) / denom;
        float w = (d00 * d21 - d01 * d20) / denom;
        float u = 1.0f - v - w;

        return new Vector3(u, v, w);
    }
}
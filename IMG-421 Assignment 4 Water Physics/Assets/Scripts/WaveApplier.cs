using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaveApplier : MonoBehaviour
{
    private MeshFilter meshFilter;
    private List<Wave> waves;

    // Cache the base Y positions so we always displace from flat, not from
    // the previous frame's displaced position (prevents drift over time)
    private float[] baseHeights;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        waves = FindObjectsOfType<Wave>().ToList();

        // Store the original flat vertex Y positions
        var vertices = meshFilter.mesh.vertices;
        baseHeights = new float[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
            baseHeights[i] = vertices[i].y;
    }

    void Update()
    {
        ApplyWaves();
    }

    void ApplyWaves()
    {
        var mesh = meshFilter.mesh;
        var vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 world = transform.TransformPoint(vertices[i]);

            float height = 0f;
            foreach (var wave in waves)
                height += wave.GetHeight(world.x, world.z);

            // Displace from base height, not from last frame's position
            vertices[i].y = baseHeights[i] + height;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}

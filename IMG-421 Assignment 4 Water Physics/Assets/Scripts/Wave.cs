using UnityEngine;

public class Wave : MonoBehaviour
{
    public enum WaveType { Bell, Perlin }

    [SerializeField] private bool on = true;
    public WaveType type = WaveType.Perlin;

    public float maxHeight = 1f;
    public float speed = 1f;
    public Vector2 offset = Vector2.zero;
    public Vector2 scale = Vector2.one;

    [Header("Perlin Settings")]
    [Tooltip("Layers of noise stacked on top of each other for more natural variation")]
    public int octaves = 4;
    [Tooltip("How much each octave contributes - lower = smoother")]
    [Range(0f, 1f)] public float persistence = 0.5f;
    [Tooltip("How much detail each octave adds - higher = more jagged")]
    public float lacunarity = 2f;

    public float GetHeight(float x, float z)
    {
        if (!on) return 0f;

        return type == WaveType.Perlin
            ? GetPerlinHeight(x, z)
            : GetBellHeight(x, z);
    }

    public float GetBellHeight(float x, float z)
    {
        Vector2 sinWave = new Vector2(
            Mathf.Sin(x / scale.x + X + Time.time * speed),
            Mathf.Sin(z / scale.y + Y + Time.time * speed)
        );
        return maxHeight * 0.5f * (sinWave.x + sinWave.y);
    }

    public float GetPerlinHeight(float x, float z)
    {
        float height = 0f;
        float amplitude = 1f;
        float frequency = 1f;
        float totalAmplitude = 0f;

        float time = Time.time * speed;

        for (int i = 0; i < octaves; i++)
        {
            // Offset each octave's sample position so they don't overlap
            float sampleX = (x / scale.x + X) * frequency + time + i * 100f;
            float sampleZ = (z / scale.y + Y) * frequency + time + i * 100f;

            // Perlin returns 0-1, remap to -1 to 1
            float noise = Mathf.PerlinNoise(sampleX, sampleZ) * 2f - 1f;

            height += noise * amplitude;
            totalAmplitude += amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }

        // Normalize so maxHeight is respected regardless of octave count
        return maxHeight * (height / totalAmplitude);
    }

    private float X => -transform.position.x / scale.x + offset.x;
    private float Y => -transform.position.z / scale.y + offset.y;
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Buoy : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float buoyancy = 1f;

    public float damping = 2f;

    private List<Wave> waves;

    void Start()
    {
        waves = FindObjectsOfType<Wave>().ToList();
    }

    void FixedUpdate()
    {
        float waterHeight = 0f;

        foreach (var wave in waves)
            waterHeight += wave.GetHeight(transform.position.x, transform.position.z);

        if (transform.position.y < waterHeight)
        {
            float submersion = waterHeight - transform.position.y;
            float force = rigidbody.mass * Physics.gravity.magnitude * buoyancy * submersion;
 
            // Damping force opposes vertical velocity, smoothing out the bounce
            float dampingForce = -rigidbody.velocity.y * damping;
 
            rigidbody.AddForceAtPosition(Vector3.up * (force + dampingForce), transform.position);
        }
    }
}
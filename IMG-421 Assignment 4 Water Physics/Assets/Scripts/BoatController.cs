using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float thrustForce = 10f;
    public float turnTorque = 1f;
    public float maxSpeed = 12f;

    void Awake()
    {
        if (!rigidbody) rigidbody = GetComponent<Rigidbody>();

        rigidbody.drag = 1.5f;
        rigidbody.angularDrag = 2f;
    }

    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (rigidbody.velocity.magnitude < maxSpeed) rigidbody.AddForce(transform.forward * v * thrustForce);

        rigidbody.AddTorque(Vector3.up * h * turnTorque);

        Vector3 euler = transform.rotation.eulerAngles;
        euler.x = Mathf.LerpAngle(euler.x, 0f, Time.fixedDeltaTime * 3f);
        euler.z = Mathf.LerpAngle(euler.z, 0f, Time.fixedDeltaTime * 3f);
        rigidbody.MoveRotation(Quaternion.Euler(euler));
    }
}

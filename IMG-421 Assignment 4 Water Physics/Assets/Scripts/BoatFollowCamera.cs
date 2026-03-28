using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFollowCamera : MonoBehaviour
{
    public Transform target;
    public Transform goalPosition;
    public float smoothSpeed = 1f;

    void LateUpdate()
    {
        Vector3 delta = goalPosition.position - transform.position;
        float distanceMultiplier = delta.x * delta.x + delta.y * delta.y + delta.z * delta.z;
        transform.position = Vector3.MoveTowards(transform.position, goalPosition.position, smoothSpeed * distanceMultiplier * Time.deltaTime);
        transform.LookAt(target);
    }
}

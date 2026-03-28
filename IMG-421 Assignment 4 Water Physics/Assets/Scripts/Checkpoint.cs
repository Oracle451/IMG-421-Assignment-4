using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public System.Action onReached;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BoatController>() != null)
        {
            Debug.Log("Checkpoint reached");

            onReached?.Invoke();

            Destroy(gameObject);
        }
    }
}

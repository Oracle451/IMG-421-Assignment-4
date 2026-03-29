using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles the checkpoint behavior so that when the player enters the checkpoint
// it fires an event that destroys the checkpoint
public class Checkpoint : MonoBehaviour
{
    // Action that will be called when the checkpoint is reached
    public System.Action onReached;

    // Called automatically when another collider enters the object
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered has a boat controller component
        if (other.GetComponent<BoatController>() != null)
        {
            // Log a message to the console saying a checkpoint is reached
            Debug.Log("Checkpoint reached");

            // Invoke the on reached event
            onReached?.Invoke();

            // Destroy this checkpoint object
            Destroy(gameObject);
        }
    }
}

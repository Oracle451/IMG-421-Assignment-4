using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// This script handles spawning checkpoints, tracking progress, displaying messages, and handling wins
public class CheckpointManager : MonoBehaviour
{
    // prefab for the checkpoint object to spawn
    public GameObject checkpointPrefab;
    // Reference to the players boat location
    public Transform boat;


    // Distance ahead of the boat where checkpoints will spawn
    public float spawnDistance = 30f;
    // Width of cone infront of boat where checkpoints can spawn
    public float coneAngle = 80f;
    // Number of checkpoints required to win
    public int totalCheckpoints = 5;

    // UI text element used to display progress messages
    public TextMeshProUGUI checkpointText;
    // How long the checkpoint message stays visible for
    public float messageDuration = 1f;
    // Scene name to load upon winning
    public string mainMenuSceneName = "MainMenu";

    // Tracks how many checkpoints have been completed
    private int currentCount = 0;
    // Reference to currently active checkpoint
    private GameObject currentCheckpoint;

    // Called when game starts
    void Start()
    {
        // Spawn the first checkpoint
        SpawnNextCheckpoint();
    }

    // Spawns the next checkpoint or triggers win if finished
    void SpawnNextCheckpoint()
    {
        // If we've reached the total number of checkpoints, trigger win
        if (currentCount >= totalCheckpoints)
        {
            StartCoroutine(WinSequence());
            return;
        }

        // Increase completed checkpoint count
        currentCount++;
        // Show UI message for current checkpoint
        ShowCheckpointMessage();

        // Get a random position in front of the boat
        Vector3 spawnPos = GetRandomPointInCone();
        // Instantiate the checkpoint prefab at that position
        currentCheckpoint = Instantiate(checkpointPrefab, spawnPos, Quaternion.identity);

        // Get the checkpoint script and assign the callback
        Checkpoint checkpoint = currentCheckpoint.GetComponent<Checkpoint>();
        // When checkpoint is reached, call OnCheckpointReached
        checkpoint.onReached = OnCheckpointReached;
    }

    // Displays checkpoint progress text and fades it out
    void ShowCheckpointMessage()
    {
        // Update Text
        checkpointText.text = $"Checkpoint {currentCount}/{totalCheckpoints}";
        // Stop and currently running routines to prevent overlap
        StopAllCoroutines();
        // Start fading text out
        StartCoroutine(ClearTextAfterDelay());
    }

    // Coroutine that runs when player completes all checkpoints
    IEnumerator WinSequence()
    {
        // Display win message
        checkpointText.text = "YOU WIN!";
        // Wait briefly before switching scenes to allow player to read message
        yield return new WaitForSeconds(1f);

        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Coroutine to fade out text
    IEnumerator ClearTextAfterDelay()
    {
        // Wait before starting fade
        yield return new WaitForSeconds(messageDuration);

        float t = 0;
        // Store original color
        Color original = checkpointText.color;

        // Gradually fade the alpha value to 0
        while (t < 1f)
        {
            t += Time.deltaTime;
            checkpointText.color = new Color(original.r, original.g, original.b, 1 - t);
            yield return null;
        }

        // Clear text and restore orginal color
        checkpointText.text = "";
        checkpointText.color = original;
    }

    // Generates randomg point in a cone infront of the boat
    Vector3 GetRandomPointInCone()
    {
        // Random angle within cone
        float angle = Random.Range(-coneAngle / 2f, coneAngle / 2f);

        // Rotate boat forward direction
        Vector3 direction = Quaternion.Euler(0, angle, 0) * boat.forward;

        // Random distance variation
        float distance = Random.Range(spawnDistance * 0.7f, spawnDistance * 1.3f);

        Vector3 pos = boat.position + direction.normalized * distance;

        // Keep checkpoint on water surface 
        pos.y = 0f;

        return pos;
    }

    // Called when checkpoint is reached
    void OnCheckpointReached()
    {
        // Spawn next checkpoint
        SpawnNextCheckpoint();
    }
}

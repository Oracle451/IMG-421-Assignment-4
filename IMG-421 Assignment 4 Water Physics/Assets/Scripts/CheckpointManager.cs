using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public GameObject checkpointPrefab;
    public Transform boat;

    public float spawnDistance = 30f;
    public float coneAngle = 80f;
    public int totalCheckpoints = 5;

    public TextMeshProUGUI checkpointText;
    public float messageDuration = 1f;
    public string mainMenuSceneName = "MainMenu";

    private int currentCount = 0;
    private GameObject currentCheckpoint;

    void Start()
    {
        SpawnNextCheckpoint();
    }

    void SpawnNextCheckpoint()
    {
        if (currentCount >= totalCheckpoints)
        {
            StartCoroutine(WinSequence());
            return;
        }

        currentCount++;
        ShowCheckpointMessage();

        Vector3 spawnPos = GetRandomPointInCone();
        currentCheckpoint = Instantiate(checkpointPrefab, spawnPos, Quaternion.identity);

        Checkpoint checkpoint = currentCheckpoint.GetComponent<Checkpoint>();
        checkpoint.onReached = OnCheckpointReached;
    }

    void ShowCheckpointMessage()
    {
        checkpointText.text = $"Checkpoint {currentCount}/{totalCheckpoints}";
        StopAllCoroutines();
        StartCoroutine(ClearTextAfterDelay());
    }

    IEnumerator WinSequence()
    {
        checkpointText.text = "YOU WIN!";
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(mainMenuSceneName);
    }

    IEnumerator ClearTextAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);

        float t = 0;
        Color original = checkpointText.color;

        while (t < 1f)
        {
            t += Time.deltaTime;
            checkpointText.color = new Color(original.r, original.g, original.b, 1 - t);
            yield return null;
        }

        checkpointText.text = "";
        checkpointText.color = original;
    }

    Vector3 GetRandomPointInCone()
    {
        // Random angle within cone
        float angle = Random.Range(-coneAngle / 2f, coneAngle / 2f);

        // Rotate boat forward direction
        Vector3 direction = Quaternion.Euler(0, angle, 0) * boat.forward;

        // Random distance variation
        float distance = Random.Range(spawnDistance * 0.7f, spawnDistance * 1.3f);

        Vector3 pos = boat.position + direction.normalized * distance;

        // Keep checkpoint on water surface (y = 0 or whatever your water level is)
        pos.y = 0f;

        return pos;
    }

    void OnCheckpointReached()
    {
        SpawnNextCheckpoint();
    }
}

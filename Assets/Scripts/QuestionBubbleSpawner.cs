using System.Collections;
using UnityEngine;

public class QuestionBubbleSpawner : MonoBehaviour
{
    public GameObject questionBubblePrefab;
    public Transform playerCamera; // Assign the center eye anchor or main camera here
    public float distanceInFront = 4.0f;

    void Start()
    {
        StartCoroutine(SpawnQuestionBubbleAfterDelay());
    }

    IEnumerator SpawnQuestionBubbleAfterDelay()
    {
        yield return new WaitForSeconds(10f);

        Vector3 spawnPosition = playerCamera.position + playerCamera.forward * distanceInFront;
        Quaternion lookRotation = Quaternion.LookRotation(spawnPosition - playerCamera.position);

        GameObject bubble = Instantiate(questionBubblePrefab, spawnPosition, lookRotation);
    }
}

using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null) return;

        // Match the camera's forward (but keep upright)
        Vector3 lookDirection = transform.position - Camera.main.transform.position;
        lookDirection.y = 0f; // keep it level
        transform.forward = lookDirection.normalized;
    }
}

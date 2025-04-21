using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    public float floatSpeed = 1f;     // How fast it floats up and down
    public float floatAmplitude = 0.1f; // How far it moves up and down

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localPosition = startPosition + new Vector3(0f, yOffset, 0f);
    }
}

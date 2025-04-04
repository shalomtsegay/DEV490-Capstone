using UnityEngine;

public class WheelChairMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;  // Speed of forward and backwards movement
    public float turnSpeed = 50f;   // Speed of turning
    public Rigidbody myRigidbody;

    public float horizontalInput; // Left and Right
    public float verticalInput; // Forward and Backwards

    public Transform Wheelchair;

    [SerializeField] private Vector3 _rotation; // For testing rotation

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        horizontalInput = Input.GetAxis("Horizontal"); // Left and Right (A & D) - For Manual Testing
        verticalInput = Input.GetAxis("Vertical"); // Forward and Backwards (W & S) - For Manual Testing

        // Using the Wheelchairs Forward direction
        Vector3 moveDirection = -transform.TransformDirection(Wheelchair.forward );
        // Apply movement
        myRigidbody.linearVelocity = moveDirection * verticalInput * moveSpeed;

        
        // Turning.
        transform.Rotate(Vector3.forward * horizontalInput * turnSpeed * Time.deltaTime);
    }
}

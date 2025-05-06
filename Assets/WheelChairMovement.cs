using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
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

    private Vector3 _startPosition; // Stores the starting position
    private int currentStep = 0;     // Keeps track and controls the number of forward movement and turn sets of the wheelchair in routeList.
    private bool isMoving = false; // Control movement state
    private bool isTurning = false; // Control turning state


    // Movement & Turn Instructions (Distance, Turn Angle
    private (float moveDistance, float turnAngle)[] routeList = {
        (3.6f, -55f),  // Move 3.6 units, turn -55 degrees to the left
        (1.4f, -45f), // Move 1 units, turn -45 degrees to the left
        //(6f, 180f), // move 6 units, turn 180 degrees to the right
        //(2f, 0f)    // Move 2 units, no turn
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(routeAfterRampDeployed());
    }

    IEnumerator routeAfterRampDeployed()
    {
        yield return new WaitForSeconds(17f); // Wait for 17 seconds
        StartNextStep();
    }


    void StartNextStep()
    {
        if (currentStep < routeList.Length)
        {
            _startPosition = transform.position; // Stores the initial position of the wheelchair
            isMoving = true;
        }
        else
        {
            stopWheelchair(); // Stop after completing all steps
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement 1st version

        // horizontalInput = Input.GetAxis("Horizontal"); // Left and Right (A & D) - For Manual Testing
        // verticalInput = Input.GetAxis("Vertical"); // Forward and Backwards (W & S) - For Manual Testing

        // Using the Wheelchairs Forward direction
        //Vector3 moveDirection = -transform.TransformDirection(Wheelchair.forward );

        // Apply movement
        //myRigidbody.linearVelocity = moveDirection * verticalInput * moveSpeed;




        // Turning.
        //transform.Rotate(Vector3.forward * horizontalInput * turnSpeed * Time.deltaTime);

        // Movement 1st version end



        if (isMoving && !isTurning)
        {
            moveForward();
        }
    }

    void moveForward()
    {
        float moveDistance = routeList[currentStep].moveDistance;
        float traveledDistance = Vector3.Distance(_startPosition, transform.position);

        if (traveledDistance >= moveDistance)
        {
            isMoving = false;
            StartCoroutine(turnLeftOrRight(routeList[currentStep].turnAngle));
            return;
        }

        // Move wheelchair forward
        Vector3 moveDirection = -transform.TransformDirection(Wheelchair.forward);
        myRigidbody.linearVelocity = moveDirection * moveSpeed;
    }

    IEnumerator turnLeftOrRight(float angle)
    {
        if (angle != 0)
        {
            isTurning = true;
            float totalRotation = 0;
            float turnDirection = Mathf.Sign(angle); // -1 for left, 1 for right

            while (Mathf.Abs(totalRotation) < Mathf.Abs(angle))
            {
                float rotationStep = turnDirection * turnSpeed * Time.deltaTime;
                transform.Rotate(Vector3.forward * rotationStep); // Relies on z axis for rotation
                totalRotation += rotationStep;
                yield return null;
            }
        }

        isTurning = false;
        currentStep++; // Move to the next routing instructions
        StartNextStep();
    }


    void stopWheelchair()
    {
        isMoving = false;
        myRigidbody.linearVelocity = Vector3.zero; // Stop movement
    }

}

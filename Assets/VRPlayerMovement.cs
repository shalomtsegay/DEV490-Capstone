using System;
using UnityEngine;
using UnityEngine.XR;

public class VRPlayerMovement : MonoBehaviour
{
    public XRNode rightController;
    public XRNode leftController;

    [Space]
    public CharacterController characterController;
    public Transform headsetTransform;

    [Space]
    public float speed = 1;
    public float rotationSpeed = 45f;

    private Vector2 inputAxis;
    private float rotationInput;

    private void Update()
    {
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(rightController);
        rightDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(leftController);
        leftDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 secondaryAxis);
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();
        MovePlayer();
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        // limit to avoid jittery inputs
        if (Mathf.Abs(rotationInput) > 0.1f)
        {
            float rotationAmount = rotationInput * rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }
    }

    private void MovePlayer()
    {
        Quaternion headRotation = Quaternion.Euler(0, headsetTransform.rotation.eulerAngles.y, 0);
        Vector3 direction = headRotation * new Vector3(inputAxis.x, 0, inputAxis.y);
        characterController.Move(direction * speed * Time.fixedDeltaTime);
    }

    private void CapsuleFollowHeadset()
    {
        Vector3 capsuleCenter = transform.InverseTransformPoint(headsetTransform.position);
        characterController.center = new Vector3(capsuleCenter.x, characterController.height / 2 + characterController.skinWidth, capsuleCenter.z);
    }
}

using UnityEngine;
using System.Collections; // Required for coroutines

public class AnimateToggle : MonoBehaviour
{
    [Header("Door Components")]
    public Animator doorAnimator; // Assign in Inspector (FrontDoor Animator)
    public AudioSource doorAudio; // Assign in Inspector (FrontDoor_Audio)

    [Header("Ramp Components")]
    public Animator rampAnimator; // Assign in Inspector (Ramp Animator)
    public AudioSource rampAudio; // Assign in Inspector (Ramp Audio)

    private bool isOpen = false;
    private bool rampDeployed = false;

    void Start()
    {
        // Ensure animators are assigned
        if (doorAnimator == null) Debug.LogError("Door Animator is NOT assigned!");
        if (rampAnimator == null) Debug.LogError("Ramp Animator is NOT assigned!");

        if (doorAudio == null) Debug.LogError("Door AudioSource is NOT assigned!");
        if (rampAudio == null) Debug.LogError("Ramp AudioSource is NOT assigned!");

        // Ensure animators are disabled at start to prevent auto-play
        if (doorAnimator != null) doorAnimator.enabled = false;
        if (rampAnimator != null) rampAnimator.enabled = false;

        if (doorAudio != null) doorAudio.Stop(); // Ensure audio does not play at start
        if (rampAudio != null) rampAudio.Stop();

        // Start coroutine to delay door opening by 15 seconds
        StartCoroutine(OpenDoorAfterDelay(5f));
    }

    IEnumerator OpenDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for 5 seconds

        if (!isOpen)
        {
            OpenDoors();
        }
    }

    public void OpenDoors()
    {
        if (doorAnimator != null)
        {
            doorAnimator.enabled = true; // Enable the Animator just before triggering
            doorAnimator.Play("FrontDriverDoorOpen"); // Trigger the door animation
            Debug.Log("Door opening animation triggered.");
        }

        if (doorAudio != null)
        {
            doorAudio.Play(); // Play the door sound
            Debug.Log("Door audio played.");
        }

        isOpen = true;

        // Start coroutine to deploy the ramp 5 seconds after the doors open
        StartCoroutine(DeployRampAfterDelay(6f));
    }

    IEnumerator DeployRampAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait 5 seconds after doors open

        if (!rampDeployed)
        {
            DeployRamp();
        }
    }

    public void DeployRamp()
    {
        if (rampAnimator != null)
        {
            rampAnimator.enabled = true; // Enable the Animator just before triggering
            rampAnimator.Play("HandicapRampOpen"); // Play the correct ramp animation
            Debug.Log("Ramp deployment animation triggered.");
        }
        else
        {
            Debug.LogError("Ramp Animator is NOT assigned in the Inspector!");
        }

        /* if (rampAudio != null)
         {
             rampAudio.Play(); // Play the ramp sound
             Debug.Log("Ramp audio played.");
         }
         else
         {
             Debug.LogError("Ramp AudioSource is NOT assigned in the Inspector!");
         }*/

        rampDeployed = true;
    }
}
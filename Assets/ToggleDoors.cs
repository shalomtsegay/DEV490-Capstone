using UnityEngine;

public class AnimateToggle : MonoBehaviour
{
    public Animator doorAnimator; // Assign FrontDoor (Animator) in Inspector
    public AudioSource doorAudio; // Assign FrontDoor_Audio in Inspector
    private bool isOpen = false;

    void Start()
    {
        if (doorAnimator != null)
        {
            // Force the Animator to start in the Idle state
            doorAnimator.Play("FrontDriverDoorIdle", 0, 0);
        }

        if (doorAudio != null)
        {
            doorAudio.Stop(); // Prevent audio from playing at start
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (!isOpen)
            {
                OpenDoors();
            }
        }
    }




    public void OpenDoors()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open"); // Trigger the animation
            Debug.Log("Door opening animation triggered.");
        }
        else
        {
            Debug.LogError("Door Animator is NOT assigned in the Inspector!");
        }

        if (doorAudio != null)
        {
            doorAudio.Play(); // Play the door sound
            Debug.Log("Door audio played.");
        }
        else
        {
            Debug.LogError("Door AudioSource is NOT assigned in the Inspector!");
        }

        isOpen = true;
    }
}
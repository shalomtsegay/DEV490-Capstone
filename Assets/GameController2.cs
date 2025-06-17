using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController2 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuestionSetup questionBubblePrefab;     // Drag QuestionSetup GameObject
    [SerializeField] private Transform playerCamera;          // Drag Main Camera or XR Rig CenterEyeAnchor

    [SerializeField] private GameObject playerController; // Assign movement script (The whole camera rig)(VRPlayerMovement)
    [SerializeField] public WheelChairMovement wheelchair;

    [Header("Quantum Restraint")]
    [SerializeField] private AnimateQuantum quantumAnimator;

    
    private void Start()
    {
        StartCoroutine(RunGameSequence());
    }

    private IEnumerator RunGameSequence()
    {
        quantumAnimator.DroppedQuantumArm();

        yield return new WaitForSeconds(5f);

        // --- Start of leaving the bus

        // Scene 2

        // QUESTION 5
        yield return ShowQuestion("DISEMBARK_RESPONSE");

        Debug.Log("NPC ready to exit.");

        // QUESTION 6
        yield return ShowQuestion("PRE_EXIT_CHECK");

        yield return new WaitForSeconds(2f);

        // QUESTION 7
        yield return ShowQuestion("SAFE_EXIT_HELP");

        yield return new WaitForSeconds(2f);

        //RAISE QUANTUM ARM

        quantumAnimator.RaiseQuantumArm();
        yield return new WaitForSeconds(6f);

        wheelchair.StartTurn(routeLeaveSteps[0].angle); // Turn / Rotate to the left to face the middle of the bus
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        wheelchair.StartMoveForward(routeLeaveSteps[0].distance); // Move forward towards the middle of the bus
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        wheelchair.StartTurn(routeLeaveSteps[1].angle); // Turn / Rotate to the left to face forward towards the bus driver seat
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        wheelchair.StartMoveForward(routeLeaveSteps[1].distance); // Move forward towards the bus driver seat and by the door
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        wheelchair.StartTurn(routeLeaveSteps[2].angle); // Turn / Rotate to the right to face the bus door
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        wheelchair.StartMoveForward(routeLeaveSteps[2].distance); // Move forward towards the door exit
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        wheelchair.StartTurn(routeLeaveSteps[3].angle); // Turn / Rotate to the right to face forward to the ramp / door
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        wheelchair.StartMoveForward(routeLeaveSteps[3].distance); // Move forward down the ramp and out th ebus
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        // QUESTION 8
        yield return ShowQuestion("GOODBYE");

        yield return new WaitForSeconds(2f);

    }

    private IEnumerator ShowQuestion(string questionID)
    {
        bool answered = false;

        playerController.SetActive(false); // Disable player movement

        questionBubblePrefab.OnAnswered += MarkAnswered;
        questionBubblePrefab.ShowQuestionByName(questionID, playerCamera);

        void MarkAnswered()
        {
            answered = true;
            questionBubblePrefab.OnAnswered -= MarkAnswered;
        }

        while (!answered)
            yield return null;

        playerController.SetActive(true); // Re-enable player movement
    }

    // Routing directions
    // You can specify which pair of values you want to use and whether you want to use distance for moving forward or angle for turning in the arguments or parameters
    // for controlling the wheelchair
    public (float distance, float angle)[] routeSteps = new (float, float)[]
    {
        (3.3f, -55f), // move 3.6 forward, rotate/turn -55 left. Positive is right. 
        (0.65f, -60f), // etc..
        (2.5f, 125f),
        (0.75f, -128f),
    };

    public (float distance, float angle)[] routeLeaveSteps = new (float, float)[]
    {
        (0.6f, -125f), // Start of leaving the bus
        (2.3f, -100f),
        (0.65f, 55f),
        (3.0f, 53f)
    };

    // This makes sure an action is done first before moving on to the next. Just call either of the two methods below with the appropriate action after you call that 
    // action. Start method has an example
    private bool wheelchairFinishedMoving() => !IsMoving(wheelchair); // Makes sure moving forward is finished before anything else
    private bool wheelchairFinishedTurning() => !IsTurning(wheelchair); // Makes sure turning is finished before doing anything else.

    private bool IsMoving(WheelChairMovement wcm) =>
        wcm.GetType().GetField("isMoving", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(wcm) as bool? ?? false;

    private bool IsTurning(WheelChairMovement wcm) =>
        wcm.GetType().GetField("isTurning", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(wcm) as bool? ?? false;


}

using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuestionSetup questionBubblePrefab;     // Drag QuestionSetup GameObject
    [SerializeField] private Transform playerCamera;          // Drag Main Camera or XR Rig CenterEyeAnchor

    [SerializeField] private GameObject playerController; // Assign movement script (The whole camera rig)(VRPlayerMovement)
    [SerializeField] public WheelChairMovement wheelchair;


    // Optional future references
    // public DoorController doorController;
    // public RampController rampController;
    // public NPCController npcController;
    // public ChatBubbleSpawner chatSpawner;

    private void Start()
    {
        StartCoroutine(RunGameSequence());
    }

    private IEnumerator RunGameSequence()
    {
        yield return new WaitForSeconds(15f);

        // Each method has its own individual action
        wheelchair.StartMoveForward(routeSteps[0].distance); // Move forward in the bus door
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        wheelchair.StartTurn(routeSteps[0].angle); // Rotate/ turn left 
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        wheelchair.StartMoveForward(routeSteps[1].distance); // Go forward past the bus driver seat
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        wheelchair.StartTurn(routeSteps[1].angle); // Rotate / turn the wheelchair to the left to straighten the wheelchair straight in the bus
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        wheelchair.StartMoveForward(routeSteps[2].distance); // Move forward in the bus and align the wheelchair to the quantum seats
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        wheelchair.StartTurn(routeSteps[2].angle); // Rotate / turn the wheelchair to the right towards the quantum wheelchair
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        wheelchair.StartMoveForward(routeSteps[3].distance); // Move forward towards the quantum seat
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        wheelchair.StartTurn(routeSteps[3].angle); // Rotate / turn the wheelchair to the left to straighten the back towards the quantum seat
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        // Bus Doors Open and Ramp Deploys

        // QUESTION 1
        yield return ShowQuestion("BUS_GREETING");

        yield return new WaitForSeconds(2f);

        // NPC loads onto the bus

        // QUESTION 2
        yield return ShowQuestion("NAV_TO_SECUREMENT");

        yield return new WaitForSeconds(2f);

        // NPC goes to securement spot

       

        // QUESTION 3
        yield return ShowQuestion("SECUREMENT_OFFER");

        yield return new WaitForSeconds(2f);

        // NPC gets into exact spot, Assist with Securement

        // QUESTION 4
        yield return ShowQuestion("DISEMBARK_INSTRUCTIONS");

        yield return new WaitForSeconds(2f);

        // No movement

        
        // QUESTION 5
        yield return ShowQuestion("DISEMBARK_RESPONSE");

        Debug.Log("NPC ready to exit.");

        // No movement

        // QUESTION 6
        yield return ShowQuestion("PRE_EXIT_CHECK");

        yield return new WaitForSeconds(2f);

        // No movement

        // Lower ramp and open doors

        // QUESTION 7
        yield return ShowQuestion("SAFE_EXIT_HELP");

        yield return new WaitForSeconds(2f);

        // Unsecure wheelchair user

        // NPC unloads

        // QUESTION 8
        yield return ShowQuestion("GOODBYE");

        yield return new WaitForSeconds(2f);

        // NPC turn around to face the bus

        //NPC says goodbye

        // End scene

        // QUESTION 9
        //yield return ShowQuestion("DECLINE_SECUREMENT");

        Debug.Log("All questions completed.");

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
        (0.8f, -128f),
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

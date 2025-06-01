using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuestionSetup questionBubblePrefab;     // Drag QuestionSetup GameObject
    [SerializeField] private Transform playerCamera;          // Drag Main Camera or XR Rig CenterEyeAnchor

    [SerializeField] private GameObject playerController; // Assign movement script (The whole camera rig)(VRPlayerMovement)


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

        // Bus Doors Open and Ramp Deploys

        // NPC rolls up to the bus
        wheelchair.StartMoveForward(routeSteps[0].distance);
        yield return new WaitUntil(() => wheelchairFinishedMoving());

        wheelchair.StartTurn(routeSteps[0].angle);
        yield return new WaitUntil(() => wheelchairFinishedTurning());

        wheelchair.stopWheelchair(); // Final stop

        // QUESTION 1
        yield return ShowQuestion("BUS_GREETING");

        yield return new WaitForSeconds(2f);

        // NPC loads onto the bus

        wheelchair.StartMoveForward(routeSteps[0].distance);
        yield return new WaitUntil(() => wheelchairFinishedMoving());

        wheelchair.StartTurn(routeSteps[0].angle);
        yield return new WaitUntil(() => wheelchairFinishedTurning());

        wheelchair.stopWheelchair(); // Final stop

        // QUESTION 2
        yield return ShowQuestion("NAV_TO_SECUREMENT");

        yield return new WaitForSeconds(2f);

        // NPC goes to securement spot

        wheelchair.StartMoveForward(routeSteps[0].distance);
        yield return new WaitUntil(() => wheelchairFinishedMoving());

        wheelchair.StartTurn(routeSteps[0].angle);
        yield return new WaitUntil(() => wheelchairFinishedTurning());

        wheelchair.stopWheelchair(); // Final stop

        // QUESTION 3
        yield return ShowQuestion("SECUREMENT_OFFER");

        yield return new WaitForSeconds(2f);

        // NPC gets into exact spot, Assist with Securement

        wheelchair.StartMoveForward(routeSteps[0].distance);
        yield return new WaitUntil(() => wheelchairFinishedMoving());

        wheelchair.StartTurn(routeSteps[0].angle);
        yield return new WaitUntil(() => wheelchairFinishedTurning());

        wheelchair.stopWheelchair(); // Final stop

        // QUESTION 4
        yield return ShowQuestion("DISEMBARK_INSTRUCTIONS");

        yield return new WaitForSeconds(2f);

        // No movement

        wheelchair.StartMoveForward(routeSteps[0].distance);
        yield return new WaitUntil(() => wheelchairFinishedMoving());

        wheelchair.StartTurn(routeSteps[0].angle);
        yield return new WaitUntil(() => wheelchairFinishedTurning());

        wheelchair.stopWheelchair(); // Final stop

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

        wheelchair.StartMoveForward(routeSteps[0].distance);
        yield return new WaitUntil(() => wheelchairFinishedMoving());

        wheelchair.StartTurn(routeSteps[0].angle);
        yield return new WaitUntil(() => wheelchairFinishedTurning());

        wheelchair.stopWheelchair(); // Final stop

        // QUESTION 8
        yield return ShowQuestion("GOODBYE");

        yield return new WaitForSeconds(2f);

        // NPC turn around to face the bus

        wheelchair.StartMoveForward(routeSteps[0].distance);
        yield return new WaitUntil(() => wheelchairFinishedMoving());

        wheelchair.StartTurn(routeSteps[0].angle);
        yield return new WaitUntil(() => wheelchairFinishedTurning());

        wheelchair.stopWheelchair(); // Final stop

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

    public WheelChairMovement wheelchair;
    // Routing directions
    // You can specify which pair of values you want to use and whether you want to use distance for moving forward or angle for turning in the arguments or parameters
    // for controlling the wheelchair
    public (float distance, float angle)[] routeSteps = new (float, float)[]
    {
        (3.6f, -55f), // move 3.6 forward, rotate/turn -55 left. Positive is right. 
        (1.4f, 45f), // etc..
        //(6f, 180f),
        //(2f, 0f)
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

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
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
        yield return new WaitForSeconds(15f);

        //QUESTION 1
        yield return ShowQuestion("BUS_GREETING");

        yield return new WaitForSeconds(2f);

        // Each method has its own individual action
        wheelchair.StartMoveForward(routeSteps[0].distance); // Move forward in the bus door
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        wheelchair.StartTurn(routeSteps[0].angle); // Rotate/ turn left 
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        // QUESTION 2
        yield return ShowQuestion("NAV_TO_SECUREMENT");

        yield return new WaitForSeconds(2f);

        wheelchair.StartMoveForward(routeSteps[1].distance); // Go forward past the bus driver seat
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        wheelchair.StartTurn(routeSteps[1].angle); // Rotate / turn the wheelchair to the left to straighten the wheelchair straight in the bus
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        wheelchair.StartMoveForward(routeSteps[2].distance); // Move forward in the bus and align the wheelchair to the quantum seats
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        wheelchair.StartTurn(routeSteps[2].angle); // Rotate / turn the wheelchair to the right towards the quantum wheelchair
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        // QUESTION 3
        yield return ShowQuestion("SECUREMENT_OFFER");

        yield return new WaitForSeconds(2f);

        wheelchair.StartMoveForward(routeSteps[3].distance); // Move forward towards the quantum seat
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        wheelchair.StartTurn(routeSteps[3].angle); // Rotate / turn the wheelchair to the left to straighten the back towards the quantum seat
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        // DROP QUANTUM ARM
        quantumAnimator.DropQuantumArm();
        yield return new WaitForSeconds(6f);

        // QUESTION 4
        yield return ShowQuestion("DISEMBARK_INSTRUCTIONS");

        yield return new WaitForSeconds(5f);

        // Load second scene
        AsyncOperation load = SceneManager.LoadSceneAsync("ShalomSave2Post");
        load.allowSceneActivation = true;
        yield return load;                 // wait for Scene2 to finish loading
        yield return new WaitForSeconds(0.5f); // give things a moment


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

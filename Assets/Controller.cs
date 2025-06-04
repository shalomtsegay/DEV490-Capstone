using System.Collections;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public WheelChairMovement wheelchair;
    // Routing directions
    // You can specify which pair of values you want to use and whether you want to use distance for moving forward or angle for turning in the arguments or parameters
    // for controlling the wheelchair

    
    public (float distance, float angle)[] routeSteps = new (float, float)[]
    {
        (3.4f, -55f), // move 3.6 forward, rotate/turn -55 left. Positive is right. 
        (0.65f, -55f), // etc..
        (3.25f, 180f),
    };

    void Start() 
    {
        StartCoroutine(ExecuteRoute());

        
    }

    private IEnumerator ExecuteRoute()
    {
        yield return new WaitForSeconds(17f); // Delay before wheelchair starts moving


        wheelchair.StartMoveForward(routeSteps[0].distance); // Move forward in the bus door
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        wheelchair.StartTurn(routeSteps[0].angle); // Rotate/ turn left 
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        wheelchair.StartMoveForward(routeSteps[1].distance); // Go forward past the bus driver seat
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish

        wheelchair.StartTurn(routeSteps[1].angle); // Rotate / turn the wheelchair to the left to straighten the wheelchair straight in the bus
        yield return new WaitUntil(() => wheelchairFinishedTurning()); // Wait for action to finish

        wheelchair.StartMoveForward(routeSteps[2].distance); // Move forward in the bus and align the wheelchair to the two straps seats
        yield return new WaitUntil(() => wheelchairFinishedMoving()); // Wait for action to finish


        wheelchair.stopWheelchair(); // Final stop
    }



    // This makes sure an action is done first before moving on to the next. Just call either of the two methods below with the appropriate action after you call that 
    // action. Start method has an example
    private bool wheelchairFinishedMoving() => !IsMoving(wheelchair); // Makes sure moving forward is finished before anything else
    private bool wheelchairFinishedTurning() => !IsTurning(wheelchair); // Makes sure turning is finished before doing anything else.

    private bool IsMoving(WheelChairMovement wcm) =>
        wcm.GetType().GetField("isMoving", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(wcm) as bool? ?? false;

    private bool IsTurning(WheelChairMovement wcm) =>
        wcm.GetType().GetField("isTurning", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(wcm) as bool? ?? false;
}

using UnityEngine;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;

public class AnimateQuantum : MonoBehaviour
{
    [Header("Quantum Arm Component")]
    public Animator quantumAnimator; // Assign in Inspector

    private bool quantumDropped = false;

    void Start()
    {
        if (quantumAnimator == null)
            Debug.LogError("Quantum Animator is NOT assigned!");

        // Prevent auto-play
        quantumAnimator.enabled = false;
    }

    /// <summary>Drop the arm (plays DropAnim)</summary>
    public void DropQuantumArm()
    {
        if (quantumAnimator == null) return;

        quantumAnimator.enabled = true;
        quantumAnimator.Play("Quantum Arm Drop");        
        Debug.Log("Quantum arm drop triggered.");
        quantumDropped = true;
    }

    /// <summary>Raise the arm (plays RaiseAnim)</summary>
    public void RaiseQuantumArm()
    {
        if (quantumAnimator == null) return;

        quantumAnimator.enabled = true;
        quantumAnimator.Play("Quantum Arm Raise");       
        Debug.Log("Quantum arm raise triggered.");
        quantumDropped = false;
    }

    public void DroppedQuantumArm()
    {
        if (quantumAnimator == null) return;

        quantumAnimator.enabled = true;
        quantumAnimator.Play("Quantum Arm Dropped");
        Debug.Log("Quantum arm dropped triggered.");
        quantumDropped = true;
    }

}

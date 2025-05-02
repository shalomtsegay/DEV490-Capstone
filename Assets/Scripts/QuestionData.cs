using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "ScriptableObjects/Question", order = 1)]

public class QuestionData : ScriptableObject
{
    public string question;
    [Tooltip("Put the correct answer in the first position, it will be randomized later")]
    public string[] answers;
}
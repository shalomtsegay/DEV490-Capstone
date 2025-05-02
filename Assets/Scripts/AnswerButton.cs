using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnswerButton : MonoBehaviour
{
    private bool isCorrect;
    [SerializeField]
    private TextMeshProUGUI answerText;

    public void SetAnswerText(string NewText)
    {
        answerText.text = NewText;
    }

    public void SetIsCorrect(bool NewBool)
    {
        isCorrect = NewBool;
    }

    public void OnClick()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer");
        }
        else
        {
            Debug.Log("Wrong Answer");
        }
    }
}

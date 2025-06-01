using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class QuestionSetup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private AnswerButton[] answerButtons;
    [SerializeField] private Canvas questionCanvas;

    private Dictionary<string, QuestionData> questionMap;
    private QuestionData currentQuestion;
    private int correctAnswerChoice;

    public event Action OnAnswered;

    private Transform trackedHead;

    private void Awake()
    {
        LoadAllQuestions();
        questionCanvas.gameObject.SetActive(false); // Hide questionbubble initially

    }

    private void LoadAllQuestions()
    {
        questionMap = new Dictionary<string, QuestionData>();
        QuestionData[] allQuestions = Resources.LoadAll<QuestionData>("Questions");

        if (allQuestions == null || allQuestions.Length == 0)
        {
            Debug.LogError("No QuestionData assets found in Resources/Questions folder! Please ensure they exist.");
        }

        foreach (QuestionData q in allQuestions)
        {
            questionMap[q.name] = q;
        }
    }

    public void ShowQuestionByName(string questionName, Transform cameraTransform)
    {
        if (!questionMap.TryGetValue(questionName, out currentQuestion))
        {
            Debug.LogError($"Question not found: {questionName}");
            return;
        }

        trackedHead = cameraTransform;

        SetQuestionValues();
        SetAnswerValues();
        PositionInFrontOfCamera(trackedHead);

        questionCanvas.gameObject.SetActive(true);
    }

    private void SetQuestionValues()
    {
        // set the question text
        questionText.text = currentQuestion.question;
    }

    private void SetAnswerValues()
    {
        // randomize answer button order
        List<string> answers = RandomizeAnswers(new List<string>(currentQuestion.answers));

        // set up the answer buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            // create a temp bool to pass to the buttons
            bool isCorrect = false;

            // if it is the correct answer, set the bool to true
            if (i == correctAnswerChoice)
            {
                isCorrect = true;
            }

            answerButtons[i].SetAnswer(answers[i], isCorrect, this);
        }
    }

    private List<string> RandomizeAnswers(List<string> originalList)
    {
        bool correctAnswerChosen = false;

        List<string> newList = new List<string>();

        for (int i =0; i < answerButtons.Length; i++)
        {
            // get a random number
            int random = UnityEngine.Random.Range(0, originalList.Count);

            // if the random number is 0, this is the correct answer
            if (random == 0 && !correctAnswerChosen)
            {
                correctAnswerChoice = i;
                correctAnswerChosen = true;
            }

            // add this to the new list
            newList.Add(originalList[random]);
            // remove this choice from the original list, it has been used
            originalList.RemoveAt(random);
        }

        return newList;
    }

    private void PositionInFrontOfCamera(Transform cameraTransform)
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 spawnPosition = cameraTransform.position + forward * 5f;
        spawnPosition.y += 0f; // Raise it to approx. head height

        transform.position = spawnPosition;
        transform.rotation = Quaternion.LookRotation(forward);
    }


    public void OnAnswerSelected()
    {
        questionCanvas.gameObject.SetActive(false);
        OnAnswered?.Invoke();
    }

    private void LateUpdate()
    {
        if (trackedHead != null && questionCanvas.gameObject.activeSelf)
        {
            PositionInFrontOfCamera(trackedHead);
        }
    }


}

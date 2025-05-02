using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionSetup : MonoBehaviour
{

    [SerializeField]
    private List<QuestionData> questions;
    private QuestionData currentQuestion;

    [SerializeField]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private AnswerButton[] answerButtons;

    [SerializeField]
    private int correctAnswerChoice;

    private void Awake()
    {
        GetQuestionAssets();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get a new question
        SelectNewQuestion();
        // set all text and values
        SetQuestionValues();
        // set all of the answer texts and correct answer value
        SetAnswerValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetQuestionAssets()
    {
        // get all questions from folder
        questions = new List<QuestionData>(Resources.LoadAll<QuestionData>("Questions"));
    }

    private void SelectNewQuestion()
    {
        // Get a random value for which question to choose
        int randomQuestionIndex = Random.Range(0, questions.Count);
        // set the question to the random index
        currentQuestion = questions[randomQuestionIndex];
        // remove the question from the list so it will not e repeated
        questions.RemoveAt(randomQuestionIndex);

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

            answerButtons[i].SetIsCorrect(isCorrect);
            answerButtons[i].SetAnswerText(answers[i]);
        }
    }

    private List<string> RandomizeAnswers(List<string> originalList)
    {
        bool correctAnswerChosen = false;

        List<string> newList = new List<string>();

        for (int i =0; i < answerButtons.Length; i++)
        {
            // get a random number
            int random = Random.Range(0, originalList.Count);

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
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI answerText;

    private bool isCorrect;
    private QuestionSetup questionBubblePrefab;

    public void SetAnswer(string text, bool correct, QuestionSetup prefab)
    {
        answerText.text = text;
        isCorrect = correct;
        questionBubblePrefab = prefab;

        // Set up click listener
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (isCorrect)
        {
            Debug.Log("Correct!");
            questionBubblePrefab.OnAnswerSelected(); // Only progress on correct
        }
        else
        {
            Debug.Log("Incorrect! Try again.");
        }
    }
}

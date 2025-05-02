using UnityEngine;
using UnityEditor;
using System.IO;


public class CSVtoSO
{
    private static string questionsCSVPath = "/Editor/CSVs/VRQuestionsV2.csv";
    private static int numberOfAnswers = 4;

    [MenuItem("Utilities/Generate Questions")]
    public static void GenerateQuestions()
    {
        Debug.Log("Generated Questions");
        string[] allLines = File.ReadAllLines(Application.dataPath + questionsCSVPath);

        string folderPath = "Assets/Resources/Questions";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length < 2 + numberOfAnswers) continue;

            // create question data
            QuestionData questionData = ScriptableObject.CreateInstance<QuestionData>();
            questionData.question = splitData[1];

            questionData.answers = new string[numberOfAnswers];
            for (int i = 0; i < numberOfAnswers; i++)
            {
                questionData.answers[i] = splitData[2 + i];
            }

            questionData.name = splitData[0].Trim(); // use first column as safe file name

            AssetDatabase.CreateAsset(questionData, $"{folderPath}/{questionData.name}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}

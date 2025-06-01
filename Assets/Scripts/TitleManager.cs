
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public string mainGameSceneName = "ShalomSave2";

    public void LoadMainGame()
    {
        SceneManager.LoadScene(mainGameSceneName);
    }
}

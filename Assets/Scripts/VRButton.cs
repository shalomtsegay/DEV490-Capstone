
using UnityEngine;

public class VRButton : MonoBehaviour
{
    public TitleManager titleManager;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            titleManager.LoadMainGame();
        }
    }
}

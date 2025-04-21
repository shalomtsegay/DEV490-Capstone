using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class ChatBubble : MonoBehaviour
{
    public static ChatBubble Instance;

    public GameObject chatBubblePrefab;


    //public static void Create(Transform parent, Vector3 localPosition, string text)
    //{
    //    Transform chatBubbleTransform = Instantiate(GameAssets.i.pfChatBubble, parent).transform;
    //    chatBubbleTransform.localPosition = localPosition;

    //    // Make it face the camera
    //    chatBubbleTransform.LookAt(Camera.main.transform);       // Face the camera
    //    chatBubbleTransform.Rotate(0f, 180f, 0f);                // Optional: flip if facing backwards

    //    chatBubbleTransform.GetComponent<ChatBubble>().Setup(text);

    //    //Destroy(chatBubbleTransform.gameObject, 15f);
    //}

    public static void Create(Transform npc, Vector3 worldOffset, string text)
    {
        // Instantiate at the correct world position (not local)
        Vector3 spawnPosition = npc.position + worldOffset;
        Transform chatBubbleTransform = Instantiate(GameAssets.i.pfChatBubble).transform;
        chatBubbleTransform.position = spawnPosition;

        // Parent to NPC so it follows them, but don’t inherit rotation/scale
        chatBubbleTransform.SetParent(npc, worldPositionStays: true);

        // Add the FaceCamera component to auto-orient toward player
        if (chatBubbleTransform.GetComponent<FaceCamera>() == null)
            chatBubbleTransform.gameObject.AddComponent<FaceCamera>();

        chatBubbleTransform.GetComponent<ChatBubble>().Setup(text);
    }

    private SpriteRenderer backgroundSpriteRenderer;
    private TextMeshPro textMeshPro;

    private void Awake()
    {
        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text (TMP)").GetComponent<TextMeshPro>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //ChatBubble.Create(transform, new Vector3(0f, 1.5f, 0f), "Sample Text try zero zero zero");

    }

    // Update is called once per frame
    private void Setup(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);

        Vector2 padding = new Vector2(7f, 2f);
        backgroundSpriteRenderer.size = textSize + padding;

        Vector3 offset = Vector3.zero;
        backgroundSpriteRenderer.transform.localPosition =
            Vector3.zero;

    }
}

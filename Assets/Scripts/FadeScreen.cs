using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2;
    public Color fadeColor;
    public Renderer render;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        render = GetComponent<Renderer>();
        if (fadeOnStart)
            FadeIn();
    }
    public void FadeIn() 
    {
        fade(1,0);
    }
    public void FadeOut() 
    {
        fade(0,1);
    }
    public void fade(float alphaIn, float alphaOut) 
    {
        StartCoroutine(FadeRoutine(alphaIn,alphaOut));
    }
    public IEnumerator FadeRoutine(float alphaIn, float alphaOut) 
    {
        float timer = 0;
        while (timer <= fadeDuration) 
        {
            Color newcolor = fadeColor;
            newcolor.a = Mathf.Lerp(alphaIn, alphaOut, timer/fadeDuration);

            render.material.SetColor("_Color", newcolor);

            timer += Time.deltaTime;
            yield return null;
        }
        Color newcolor2 = fadeColor;
        newcolor2.a =alphaOut;
        render.material.SetColor("_Color", newcolor2);
    }
}

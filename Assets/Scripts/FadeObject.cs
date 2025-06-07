using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class was entirely provided by Github Copilot

public class FadeObject : MonoBehaviour
{
    public float fadeDuration = 2.0f; //Duration of the fade effect in seconds
    private Renderer[] childRenderers;
    private Color[] originalColors;

    // Start is called before the first frame update
    void Start()
    {
        childRenderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[childRenderers.Length];

        for (int i = 0; i < childRenderers.Length; i++)
        {
            originalColors[i] = childRenderers[i].material.color;
        }
    }

    public void FadeOut()
    {
        if (childRenderers != null && childRenderers.Length > 0)
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);

            for (int i = 0; i < childRenderers.Length; i++)
            {
                Color newColor = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, alpha);
                childRenderers[i].material.color = newColor;
            }

            yield return null;
        }

        // Ensure all objects are fully transparent at the end of the fade
        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].material.color = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, 0.0f);
        }
    }
}

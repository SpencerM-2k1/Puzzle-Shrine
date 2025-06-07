using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOnTop : MonoBehaviour
{
    public int renderQueue = 4000; // Default render queue for UI Overlay is 4000

    void Start()
    {
        SetRenderQueueRecursively(transform);
    }

    void SetRenderQueueRecursively(Transform parent)
    {
        Debug.Log("Setting renderQueue of " + parent.name);
        Renderer renderer = parent.GetComponent<Renderer>();
        if (renderer != null)
        {
            foreach (Material mat in renderer.materials)
            {
                mat.renderQueue = renderQueue;
            }
        }

        foreach (Transform child in parent)
        {
            SetRenderQueueRecursively(child);
        }
    }
}

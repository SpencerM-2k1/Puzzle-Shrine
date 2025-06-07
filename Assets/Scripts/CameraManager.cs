using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wilberforce.Colorblind))]
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("VisualManager initialized!");
            instance = this;
            //Camera is NOT persistent
        }
        // else if (instance != this)
        // {
        //     Destroy(this.gameObject);
        // }
    }

    void Start()
    {
        VisualManager.applyColorblindFilter();
    }

    // OnDestroy, clear the singleton
    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;        
        }
    }
}

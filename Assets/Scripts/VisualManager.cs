using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    //Singleton
    public static VisualManager instance = null;

    public enum ColorblindFilter {
        none,
        protanopia,
        deuteranopia,
        tritanopia
    }

    public ColorblindFilter colorblindFilter = ColorblindFilter.none;
    
    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("VisualManager initialized!");
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // OnDestroy, clear the singleton
    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;        
        }
    }

    public static void changeColorblindFilter(ColorblindFilter filter)
    {
        instance.colorblindFilter = filter;
    }

    public static void applyColorblindFilter()
    {
        switch(instance.colorblindFilter)
        {
            case ColorblindFilter.none:
                CameraManager.instance.GetComponent<Wilberforce.Colorblind>().Type = 0;
                break;

            case ColorblindFilter.protanopia:
                CameraManager.instance.GetComponent<Wilberforce.Colorblind>().Type = 1;
                break;
                
            case ColorblindFilter.deuteranopia:
                CameraManager.instance.GetComponent<Wilberforce.Colorblind>().Type = 2;
                break;
                
            case ColorblindFilter.tritanopia:
                CameraManager.instance.GetComponent<Wilberforce.Colorblind>().Type = 3;
                break;
        }
        
    }
}

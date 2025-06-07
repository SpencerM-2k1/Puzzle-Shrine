using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public static LocationManager instance;
    
    [SerializeField] List<LocationSO> locationStack;
    [SerializeField] LocationSO fallbackLoc;

    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("LocationManager initialized!");
            instance = this;
            //Camera is NOT persistent
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

    //Stack operations
    public void pushLocation(LocationSO loc)
    {
        locationStack.Add(loc);
        
        //Update HUD
        HUDManager.setLocationText(getCurrentLocation());

        //Update Instructions
        HUDManager.setInstructionText(getCurrentLocation());
    }

    public void popLocation()
    {
        if (locationStack.Count < 1)
        {
            Debug.LogError("LocationManager: No locations left in locationStack! Pop request ignored.");
            return;
        }

        LocationSO poppedLoc = locationStack[locationStack.Count - 1];
        locationStack.RemoveAt(locationStack.Count - 1);

        //Update HUD
        HUDManager.setLocationText(getCurrentLocation());

        //Update Instructions
        HUDManager.setInstructionText(getCurrentLocation());
    }

    public LocationSO getCurrentLocation()
    {
        if (locationStack.Count > 0)
        {
            return locationStack[locationStack.Count - 1];
        }
        Debug.LogError("LocationManager: No locations left in locationStack! Returning default location fallback...");
        return fallbackLoc;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    //Singleton
    public static HUDManager instance;

    public TMP_Text timerText;
    public TMP_Text locationText;
    public GameObject inventoryBar;
    public Image itemDisplay;
    public TMP_Text itemStackDisplay;
    public GameObject prevItemButton;
    public GameObject nextItemButton;
    public GameObject crosshair;
    public GameObject instructionsBox;
    public TMP_Text instructionsText;

    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("HUDManager initialized!");
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

    void Start()
    {
        //Request starting location
        setLocationText(LocationManager.instance.getCurrentLocation());
        // setCamera();
    }

    //Timer functions
    public static void setTimerText(int minutes, int seconds, int milliseconds)
    {
        instance.timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    //Location functions
    public static void setLocationText(LocationSO loc)
    {
        instance.locationText.text = loc.displayName;
        instance.locationText.color = loc.textColor;
    }

    public static void setInstructionText(LocationSO loc)
    {
        instance.instructionsText.text = loc.instructions;

        if (loc.instructions == "")
        {
            instance.instructionsBox.SetActive(false);
        }
        else
        {
            instance.instructionsBox.SetActive(true);
        }
    }

    //Clean deletion (call this instead of destroying the gameObject directly)
    public void destroyHUD()
    {
        Destroy(this.gameObject);
    }

    void setCamera()
    {
        GetComponent<Canvas>().worldCamera = CameraManager.instance.GetComponent<Camera>();
    }

    //Set inventory sprite
    public static void setItemDisplay(InventoryItem item, bool showPrevNext)
    {
        Sprite newImage = item.icon;
        int newStackSize = item.stackSize;

        inventoryBarActive(true);
        instance.itemDisplay.sprite = newImage;

        if (newStackSize > 1)
        {
            instance.itemStackDisplay.text = newStackSize.ToString();
        }
        else
        {
            instance.itemStackDisplay.text = "";
        }
        inventoryPrevNextActive(showPrevNext);
    }

    //Set inventory bar visibility
    public static void inventoryBarActive(bool isActive)
    {
        instance.inventoryBar.SetActive(isActive);
    }

    public static void inventoryPrevNextActive(bool isActive)
    {
        instance.prevItemButton.SetActive(isActive);
        instance.nextItemButton.SetActive(isActive);
    }

    //Hide/unhide objects when pausing
    public static void onPause()
    {
        instance.crosshair.SetActive(false);
    }

    //Hide/unhide objects when pausing
    public static void onUnpause()
    {
        instance.crosshair.SetActive(true);
    }
}

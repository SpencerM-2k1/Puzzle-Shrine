using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    PauseMenu pauseMenu;
    public static PauseManager instance;

    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("VisualManager initialized!");
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

    void Start()
    {
        pauseMenu = this.GetComponent<PauseMenu>();
    }

    static public void pauseGame()
    {
        // if (!instance.pauseMenu.isPaused)
        // {
            // instance.pauseMenu.gameObject.SetActive(true);
            instance.pauseMenu.openMenu();
        // }
    }
}

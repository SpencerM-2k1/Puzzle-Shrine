using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Singleton for player character. Used to find player in scene, particularly
    //when persistent objects are involved
    //(Should be automatically set to null if stored singleton is destroyed)
    public static PlayerManager instance;

    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("PlayerManager initialized!");
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
}

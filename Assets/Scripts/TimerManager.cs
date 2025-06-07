using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    //Singleton
    public static TimerManager instance;
    
    private float startTime;
    private bool isRunning = false;

    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("TimerManager initialized!");
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
        startTime = Time.time;
        isRunning = true;
    }


    // RARE instance of a manager actually using the update function
    void Update()
    {
        if (isRunning)
        {
            float t = Time.time - startTime;

            int minutes = ((int)t / 60);
            int seconds = ((int)t % 60);
            int milliseconds = (int)((t * 1000) % 1000);

            if (HUDManager.instance != null)
                HUDManager.setTimerText(minutes, seconds, milliseconds);
        }
    }

    //Hides transform, forces defaults
    void OnValidate()
    {
        transform.position = new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localScale = new Vector3(1f, 1f, 1f);
        
        transform.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
        // transform.hideFlags = 0;
    }

    public string getCurrentTime()
    {
        float t = Time.time - startTime;

        int minutes = ((int)t / 60);
        int seconds = ((int)t % 60);
        int milliseconds = (int)((t * 1000) % 1000);

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//USE TIMERMANAGER INSTEAD

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    private float startTime;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            float t = Time.time - startTime;

            int minutes = ((int)t / 60);
            int seconds = ((int)t % 60);
            int milliseconds = (int)((t * 1000) % 1000);

            timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        }
    }
}

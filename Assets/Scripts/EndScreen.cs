using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalTimeText;

    void Start()
    {
        if (PlayerManager.instance != null)
            Destroy(PlayerManager.instance.gameObject);
        
        setFinalTime();
    }

    public void setFinalTime()
    {
        if (TimerManager.instance == null)
        {
            //Fallback
            Debug.LogError("EndScreen: No TimerManager found! Using fallback for final time display");
            finalTimeText.text = "Final Time: " + "ERROR, NOT FOUND";
        }
        else
        {
            //Apparently referencing finalTimeText like this causes an error but also it properly assigns the text anyways
            //I don't have the time to fix meaningless errors right now. I hate Unity.
            //  UPDATE: Turns out I had a duplicate EndScreen instance in the scene. I might be stupid. (Still hate Unity tho)
            finalTimeText.text = "Final Time: " + TimerManager.instance.getCurrentTime();
        }
    }

    //Scene Navigation
    public void returnToTitle()
    {
        Destroy(TimerManager.instance.gameObject); //Ensure that timer is reset on a new game
        SceneManager.LoadScene("Scenes/StartScreen");
    }

    public void quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    //Buttons
    public void newGame()
    {
        SceneManager.LoadScene("Scenes/MainScene");
    }

    public void settings()
    {
        SceneManager.LoadScene("Scenes/Settings");
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

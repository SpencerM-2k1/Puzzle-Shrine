using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wilberforce;

public class GamestateManager : MonoBehaviour
{
    //Singleton for gamestate. Handles whether the player is in a menu, in the
    //game environment, etc.
    //(Should be automatically set to null if stored singleton is destroyed)
    //(...that shouldn't ever happen, though.)
    public static GamestateManager instance;

    public enum Gamestate {
        firstPerson,
        inMenu,
        puzzleBox
    }

    [SerializeField] Gamestate initialState = Gamestate.inMenu;
    [SerializeField] Gamestate _state = Gamestate.inMenu;
    public static Gamestate state {
        get {return instance._state;}
        private set {instance._state = value;}
    }

    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("GamestateManager initialized!");
            instance = this;
            setState(this.initialState);
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            setState(this.initialState);
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

    //Hides transform, forces defaults
    void OnValidate()
    {
        transform.position = new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localScale = new Vector3(1f, 1f, 1f);
        
        transform.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
        // transform.hideFlags = 0;
    }

    public static void setState(Gamestate newState)
    {
        state = newState;
        Debug.Log("Setting state to " + newState);
        switch (newState)
        {
            case Gamestate.firstPerson:
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case Gamestate.inMenu:
                Cursor.lockState = CursorLockMode.None;
                PlayerManager.instance.GetComponent<PlayerMovement>().haltMovement();
                break;
            case Gamestate.puzzleBox:
                Cursor.lockState = CursorLockMode.None;
                PlayerManager.instance.GetComponent<PlayerMovement>().haltMovement();
                break;
        }
    }

    // Start is called before the first frame update
    // void Start()
    // {
    //     VisualManager.applyColorblindFilter();
    // }
}

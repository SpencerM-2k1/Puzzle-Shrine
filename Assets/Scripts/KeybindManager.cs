using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeybindManager : MonoBehaviour
{
    //Singleton for keybind/input system. Anything involving custom keybinds 
    // should reference this object.
    //(Should be automatically set to null if stored singleton is destroyed)
    //(...that shouldn't ever happen, though.)
    public static KeybindManager instance;

    //Prevents rebinds while a rebind is currently ongoing
    public static bool rebindLock = false;

    public InputActionAsset inputActions; // Reference to your InputActions asset

    public float mouseSensitivity = 100.0f;
    
    // IN-EDITOR USE ONLY: Reset binding overrides
    // For in-game use, call ResetBindings()
    [SerializeField] bool resetBindings = false;
    
    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("KeybindManager initialized!");
            instance = this;
            LoadBindings();
            loadSensitivity();
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

    //Hides transform, forces defaults
    void OnValidate()
    {
        transform.position = new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localScale = new Vector3(1f, 1f, 1f);
        
        transform.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
        // transform.hideFlags = 0;
    }

    //Save/Load/Reset binding overrides
    static public void SaveBindings()
    {
        string bindings = instance.inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("KeyBindings", bindings);
        PlayerPrefs.Save();
    }

    static public void LoadBindings()
    {
        if (PlayerPrefs.HasKey("KeyBindings"))
        {
            string bindings = PlayerPrefs.GetString("KeyBindings");
            instance.inputActions.LoadBindingOverridesFromJson(bindings);
        }
    }

    static public void ResetBindings()
    {
        if (instance != null)
            instance.inputActions.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey("KeyBindings");
        PlayerPrefs.Save();
    }

    //Mouse sensitivity
    static public void setSensitivity(float newSensitivity)
    {
        instance.mouseSensitivity = newSensitivity;
        PlayerPrefs.SetFloat("MouseSensitivity", newSensitivity);
        PlayerPrefs.Save();
    }

    static public void loadSensitivity()
    {
        if (PlayerPrefs.HasKey("MouseSensitivity"))
        {
            float sensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
            instance.mouseSensitivity = sensitivity;
        }
    }


    //Actions
    public void Move(InputAction.CallbackContext context)
    {
        if (GamestateManager.state != GamestateManager.Gamestate.firstPerson)
            return;
        // Debug.Log(context.ReadValue<Vector2>());
        PlayerManager.instance.GetComponent<PlayerMovement>().setMovement(context.ReadValue<Vector2>());
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (GamestateManager.state != GamestateManager.Gamestate.firstPerson)
            return;
        // Debug.Log(context);
        if (context.phase != InputActionPhase.Started)
            return;
        PlayerManager.instance.GetComponent<PlayerLook>().interact();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;
        if (GamestateManager.state != GamestateManager.Gamestate.firstPerson)
            return;
        // Debug.Log(context);
        PlayerManager.instance.GetComponent<PlayerMovement>().jump();
    }

    public void PauseMenu(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;
        if (GamestateManager.state != GamestateManager.Gamestate.firstPerson)
            return;
        if (PauseManager.instance == null)
            return;
        Debug.Log("Pause should have been successful");
        PauseManager.pauseGame();
    }

    public void prevItem(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;
        if (GamestateManager.state != GamestateManager.Gamestate.firstPerson)
            return;
        if (InventoryManager.instance == null)
            return;
        InventoryManager.prevItem();
    }

    public void nextItem(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;
        if (GamestateManager.state != GamestateManager.Gamestate.firstPerson)
            return;
        if (InventoryManager.instance == null)
            return;
        InventoryManager.nextItem();
    }

    public void back(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;
        switch (GamestateManager.state)
        {
            case (GamestateManager.Gamestate.puzzleBox):
                PuzzleboxManager.quitPuzzle();
                break;
        }
    }

    public void use(InputAction.CallbackContext context)
    {
        if (GamestateManager.state != GamestateManager.Gamestate.firstPerson)
            return;
        if (context.phase != InputActionPhase.Started)
            return;
        InventoryManager.useCurrentItem();
    }

    //Inspector button: reset bindings
    void OnDrawGizmos()
    {
        if (resetBindings)
        {
            ResetBindings();
            resetBindings = false;
        }
    }
}

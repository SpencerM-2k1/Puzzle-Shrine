using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class KeybindButton : MonoBehaviour
{
    // public string actionName;
    // public string keybindName;

    KeybindManager keybindManager;
    InputActionAsset inputActions;
    // public InputAction buttonAction;
    public InputActionReference buttonAction;

    [SerializeField] TMP_Text rebindText;

    //Handle composite input (curse you WASD)
    [SerializeField] bool isComposite = false;
    [SerializeField] string compositeElement;

    // Start is called before the first frame update
    void Start()
    {
        keybindManager = KeybindManager.instance;
        inputActions = keybindManager.inputActions;

        //Update display to match control config
        if (isComposite)
        {
            int bindingIndex = buttonAction.action.bindings.IndexOf(x => x.isPartOfComposite && x.name == compositeElement);
            rebindText.text = buttonAction.action.bindings[bindingIndex].ToDisplayString();
        }
        else
        {
            rebindText.text = buttonAction.action.GetBindingDisplayString();
        }
    }

    // Rebind a key
    public void StartRebinding()
    {
        if (KeybindManager.rebindLock)
        {
            Debug.LogWarning($"Rebinding is currently locked.");
            return;
        }

        if (isComposite)
        {
            CompositeRebind();
        }
        else
        {
            Rebind();
        }
    }

    public void Rebind()
    {
        KeybindManager.rebindLock = true; //lock

        // InputAction actionToRebind = inputActions.FindAction(actionName);
        InputAction actionToRebind = buttonAction.action;

        // if (actionToRebind == null)
        // {
        //     Debug.LogError($"Action {actionName} not found!");
        //     KeybindManager.rebindLock = false; //unlock
        //     return;
        // }

        actionToRebind.Disable();
        rebindText.text = "...";

        // KeybindManager.rebindLock = false; //unlock
        //     return;

        actionToRebind.PerformInteractiveRebinding()
            .OnComplete(operation =>
            {
                rebindText.text = actionToRebind.GetBindingDisplayString();
                actionToRebind.Enable();
                KeybindManager.SaveBindings(); // Save updated bindings
                operation.Dispose();
            })
            .Start();
        KeybindManager.rebindLock = false; //unlock
        KeybindManager.LoadBindings();
    }

    public void CompositeRebind()
    {
        KeybindManager.rebindLock = true; //lock

        // InputAction actionToRebind = inputActions.FindAction(actionName);
        InputAction actionToRebind = buttonAction.action;
        int bindingIndex = actionToRebind.bindings.IndexOf(x => x.isPartOfComposite && x.name == compositeElement);

        // foreach (var binding in buttonAction.action.bindings)
        // {
        //     Debug.Log(binding.name);
        // }
        if (bindingIndex == -1)
        {
            Debug.LogError($"No binding found for composite part '{compositeElement}'");
            KeybindManager.rebindLock = false; //unlock
            return;
        }
        
        actionToRebind.Disable();
        rebindText.text = "...";

        actionToRebind.PerformInteractiveRebinding()
            .WithTargetBinding(bindingIndex)
            .OnComplete(operation =>
            {
                rebindText.text = actionToRebind.bindings[bindingIndex].ToDisplayString();
                actionToRebind.Enable();
                Debug.Log($"Rebound '{compositeElement}' to {actionToRebind.bindings[bindingIndex].effectivePath}");
                KeybindManager.SaveBindings(); // Save updated bindings
                operation.Dispose();
            })
            .Start();
        KeybindManager.rebindLock = false; //unlock
        KeybindManager.LoadBindings();
    }
}

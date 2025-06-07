using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject menuRoot;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject keybindMenu;

    [SerializeField] Slider sensitivitySlider;
    [SerializeField] Slider ambienceSlider;
    [SerializeField] Slider sfxSlider;

    [SerializeField] TMP_Text colorblindButtonText;

    GameObject currentMenu;

    [SerializeField] bool isSettingsMenu = false; //Settings menu is a variant of the pause menu

    GamestateManager.Gamestate unpausedState;

    public bool isPaused = false;

    void Start()
    {
        currentMenu = mainMenu;
        if (!isSettingsMenu)
        {
            // this.gameObject.SetActive(false);
            this.menuRoot.SetActive(false);
        }
        // setCamera();
        sensitivitySlider.value = KeybindManager.instance.mouseSensitivity;
        ambienceSlider.value = AudioManager.instance.ambienceVolume;
        sfxSlider.value = AudioManager.instance.sfxVolume;

        // openMenu();
    }

    public void gotoMainMenu()
    {
        currentMenu.SetActive(false);
        mainMenu.SetActive(true);
        currentMenu = mainMenu;
    }

    public void gotoKeybindMenu()
    {
        currentMenu.SetActive(false);
        keybindMenu.SetActive(true);
        currentMenu = keybindMenu;
    }

    public void exitSettings()
    {
        Debug.Log("Exit not yet implemented");
        if (isSettingsMenu)
            returnToTitle();
        else
            closeMenu();
    }

    //Main menu
    
    //Cycle Colorblind filter
    public void cycleColorblindFilter()
    {
        string buttonText = "";
        // Debug.Log("Reminder: Colorblind filters are for testing, and are not an accessibility feature on their own!");
        switch(VisualManager.instance.colorblindFilter)
        {
            case VisualManager.ColorblindFilter.none:
                VisualManager.changeColorblindFilter(VisualManager.ColorblindFilter.protanopia);
                VisualManager.applyColorblindFilter();
                buttonText = "Protanopia";
                break;

            case VisualManager.ColorblindFilter.protanopia:
                VisualManager.changeColorblindFilter(VisualManager.ColorblindFilter.deuteranopia);
                VisualManager.applyColorblindFilter();
                buttonText = "Deuteranopia";
                break;

            case VisualManager.ColorblindFilter.deuteranopia:
                VisualManager.changeColorblindFilter(VisualManager.ColorblindFilter.tritanopia);
                VisualManager.applyColorblindFilter();
                buttonText = "Tritanopia";
                break;

            case VisualManager.ColorblindFilter.tritanopia:
                VisualManager.changeColorblindFilter(VisualManager.ColorblindFilter.none);
                VisualManager.applyColorblindFilter();
                buttonText = "None";
                break;
        }
        colorblindButtonText.text = buttonText;
    }

    //Settings Menu only
    public void returnToTitle()
    {
        SceneManager.LoadScene("Scenes/StartScreen");
    }

    //Open/close menu
    public void openMenu()
    {
        // this.gameObject.SetActive(true);
        this.menuRoot.SetActive(true);
        HUDManager.onPause();
        isPaused = true;
        unpausedState = GamestateManager.state;
        GamestateManager.setState(GamestateManager.Gamestate.inMenu);
    }

    public void closeMenu()
    {
        GamestateManager.setState(unpausedState);
        HUDManager.onUnpause();
        isPaused = false;
        // this.gameObject.SetActive(false);
        this.menuRoot.SetActive(false);
    }

    void setCamera()
    {
        canvas.worldCamera = CameraManager.instance.GetComponent<Camera>();
    }

    public void setAmbienceVolume(float value)
    {
        AudioManager.SetAmbienceVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        AudioManager.SetSFXVolume(value);
    }

    public void SetMouseSensitivity(float value)
    {
        KeybindManager.setSensitivity(value);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalPuzzle : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip selectSound;
    [SerializeField] AudioClip cancelSound;
    [SerializeField] AudioClip winSound;

    //Game object to sort interface elements under
    // (Used to get around complications with disabling this gameObject)
    public Transform interfaceRoot;
    
    public virtual void powerOn()
    {
        Debug.LogError("TerminalPuzzle.powerOn() called from parent class! Please check the code!");
    }

    public virtual void powerOff()
    {
        Debug.LogError("TerminalPuzzle.powerOff() called from parent class! Please check the code!");
    }

    public void playSelectSound()
    {
        audioSource.PlayOneShot(selectSound);
    }

    public void playCancelSound()
    {
        audioSource.PlayOneShot(cancelSound);
    }

    public void playWinSound()
    {
        audioSource.PlayOneShot(winSound);
    }
}

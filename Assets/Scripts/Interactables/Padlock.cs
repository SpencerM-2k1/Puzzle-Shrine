using System.Collections.Generic;
using UnityEngine;

public class Padlock : MonoBehaviour
{
    public LockedDoor door;
    public bool isLocked = true;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> unlockSounds = new List<AudioClip>();

    [SerializeField] GameObject lockObject;

    public bool onInteract()
    {
        lockObject.SetActive(false);
        isLocked = false;
        playUnlockSound();
        door.tryUnlock();
        return true;
    }

    void playUnlockSound()
    {
        int randomValue = Random.Range(0, unlockSounds.Count);
        audioSource.PlayOneShot(unlockSounds[randomValue]);
    }

}

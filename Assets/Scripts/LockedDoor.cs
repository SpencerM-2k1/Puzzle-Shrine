using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] List<Padlock> locks = new List<Padlock>();
    [SerializeField] List<GameObject> doors = new List<GameObject>();
    
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip unlockSound;

    public void Start()
    {
        foreach(Padlock padlock in locks)
        {
            padlock.door = this;
        }
    }

    public void tryUnlock()
    {
        if (checkUnlock())
        {
            foreach(GameObject door in doors)
            {
                audioSource.PlayOneShot(unlockSound);
                door.SetActive(false);
            }
        }
    }

    bool checkUnlock()
    {
        foreach(Padlock padlock in locks)
        {
            if (padlock.isLocked == true)
            {
                return false;
            }
        }

        return true;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PadlockHitbox : SecondaryInteractable
{
    Padlock padlock;

    void Start()
    {
        padlock = GetComponentInParent<Padlock>();
    }

    public override bool secondaryInteract()
    {
        return padlock.onInteract();
    }
}

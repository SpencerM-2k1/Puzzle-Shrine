using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    [SerializeField] LocationSO location;
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerInstance = other.gameObject.GetComponent<PlayerMovement>();
        if (playerInstance != null)
        {
            LocationManager.instance.pushLocation(this.location);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement playerInstance = other.gameObject.GetComponent<PlayerMovement>();
        if (playerInstance != null)
        {
            LocationManager.instance.popLocation();
        }
        
    }
}

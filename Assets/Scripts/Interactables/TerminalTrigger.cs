using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalTrigger : MonoBehaviour
{
    [SerializeField] TerminalPuzzle terminal;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerInstance = other.gameObject.GetComponent<PlayerMovement>();
        if (playerInstance != null)
        {
            Debug.Log("Player entered the region!");
            terminal.powerOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement playerInstance = other.gameObject.GetComponent<PlayerMovement>();
        if (playerInstance != null)
        {
            Debug.Log("Player exited the region!");
            terminal.powerOff();
        }
        
    }
}

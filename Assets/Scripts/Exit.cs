using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerManager playerInstance = other.gameObject.GetComponent<PlayerManager>();
        if (playerInstance != null)
        {
            tryExit();
        }
    }

    private void tryExit()
    {
        HUDManager.instance.destroyHUD();
        InventoryManager.instance.destroyInventory();
        SceneManager.LoadScene("Scenes/EndScreen");
    }
}

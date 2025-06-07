using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleboxTrigger : MonoBehaviour
{
    [SerializeField] PuzzleboxManager puzzlebox;

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager playerInstance = other.gameObject.GetComponent<PlayerManager>();
        if (playerInstance != null)
        {
            startPuzzlebox();
        }
    }

    private void startPuzzlebox()
    {
        puzzlebox.gameObject.SetActive(true);
        GamestateManager.setState(GamestateManager.Gamestate.inMenu);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangramPlayer : TerminalPuzzle
{
    [SerializeField] List<TangramPiece> pieces;

    //Win screen display
    [SerializeField] private GameObject winScreen;
    public static PuzzleboxManager instance;

    //Reward
    [SerializeField] InventoryItem rewardItem;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(TangramPiece piece in pieces)
        {
            piece.setPlayer(this);
        }
        powerOff();
    }

    //Check for a win
    public bool checkWinState()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if(!pieces[i].checkWin())
            {
                return false;
            }
        }
        Debug.Log("A win was detected!");
        winScreen.SetActive(true);
        this.gameObject.SetActive(false);

        if (rewardItem != null)
            InventoryManager.addItem(rewardItem);
        this.playWinSound();
        return true;
    }
    
    public override void powerOn()
    {
        if (!this.gameObject.activeInHierarchy)
            return;
        Debug.Log("Enabling screen!");
        interfaceRoot.gameObject.SetActive(true);
        interfaceRoot.gameObject.GetComponent<Animator>().Play("TerminalOn");
    }

    public override void powerOff()
    {
        if (!this.gameObject.activeInHierarchy)
            return;
        Debug.Log("Disabling screen!");
        interfaceRoot.gameObject.GetComponent<Animator>().Play("TerminalOff");
    }
}

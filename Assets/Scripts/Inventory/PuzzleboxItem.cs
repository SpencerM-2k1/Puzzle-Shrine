using UnityEngine;

public class PuzzleboxItem : InventoryItem
{
    public override string itemID {
        get {return "puzzleboxItem";}
    }

    public override void use()
    {
        Debug.Log("Starting puzzle...");
        PuzzleboxManager.startPuzzle();
    }
}

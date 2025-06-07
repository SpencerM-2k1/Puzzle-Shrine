using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuChoiceButton : ButtonManager
{
    [SerializeField] public SudokuPlayer player;
    
    public int choiceValue = 0;

    void Start() {
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }

    //When player clicks on this choice
    public override void onButtonClick()
    {
        // Debug.Log("Island " + x + ", " + y + " received click!");
        player.choiceClicked(this);
    }
}

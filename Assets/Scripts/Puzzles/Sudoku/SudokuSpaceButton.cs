using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SudokuSpaceButton : ButtonManager
{
    [SerializeField] public SudokuPlayer player;
    
    //If space is locked, it is pre-determined at the start of the game
    public bool isLocked = false;

    [SerializeField] public int value = 0;
    [SerializeField] TMP_Text numDisplay;
    [SerializeField] GameObject hitbox;

    [SerializeField] Color lockedColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        if (value != 0)
            isLocked = true;
        updateDisplay();
        if (isLocked)
        {
            hitbox.SetActive(false);
            numDisplay.color = lockedColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setValue(int newValue)
    {
        value = newValue;
        updateDisplay();
        player.checkWin();
    }

    void updateDisplay()
    {
        if (value != 0)
            numDisplay.text = value.ToString();
    }


    //When player clicks on this space
    public override void onButtonClick()
    {
        player.spaceClicked(this);
    }
}

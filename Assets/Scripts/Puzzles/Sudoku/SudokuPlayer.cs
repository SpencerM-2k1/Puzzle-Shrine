using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuPlayer : TerminalPuzzle
{
    [SerializeField] List<SudokuChoiceButton> choiceButtons;
    [SerializeField] List<SudokuSpaceButton> spaceButtons;
    SudokuChoiceButton currentChoice;
    SudokuSpaceButton currentSpace;

    SudokuSpaceButton[,] grid = new SudokuSpaceButton[gridWidth, gridHeight];
    const int gridWidth = 4;
    const int gridHeight = 4;

    [SerializeField] GameObject winScreen;


    //Reward
    [SerializeField] InventoryItem rewardItem;

    // Start is called before the first frame update
    void Start()
    {
        foreach (SudokuChoiceButton button in choiceButtons)
        {
            button.player = this;
        }
        //NOTE: counter should equal spaceButtons.Count, and should equal gridWidth*gridHeight
        int counter = 0;
        for(int j = 0; j < gridHeight; j++)
        {
            for(int i = 0; i < gridHeight; i++)
            {
                spaceButtons[counter].player = this;
                grid[i, j] = spaceButtons[counter];
                counter++;
            }
        }
        powerOff();
    }

    public void choiceClicked(SudokuChoiceButton button)
    {
        Debug.Log("Choice button " + button.choiceValue + " clicked");
        //Only allow one choice to be selected at a time
        if (currentChoice != null)
        {
            currentChoice.deselect();
        }

        //Choice re-selected
        if (currentChoice == button)
        {
            // currentChoice.deselect();
            currentChoice = null;
            this.playCancelSound();
            return;
        }

        if (currentSpace == null)
        {
            //If not space is selected, select a choice
            currentChoice = button;
            currentChoice.select();
        }
        else
        {
            //If a space was already selected, assign a choice
            setValue(button, currentSpace);
        }
        this.playSelectSound();
    }

    public void spaceClicked(SudokuSpaceButton button)
    {
        Debug.Log("Space button " + button.value + " clicked");
        //Only allow one choice to be selected at a time
        if (currentSpace != null)
        {
            currentSpace.deselect();
        }

        //Choice re-selected
        if (currentSpace == button)
        {
            // currentChoice.deselect();
            currentSpace = null;
            this.playCancelSound();
            return;
        }

        if (currentChoice == null)
        {
            //If not space is selected, select a choice
            currentSpace = button;
            currentSpace.select();
        }
        else
        {
            //If a space was already selected, assign a choice
            setValue(currentChoice, button);
        }
        this.playSelectSound();
    }

    void setValue(SudokuChoiceButton choice, SudokuSpaceButton space)
    {
        space.setValue(choice.choiceValue);
        if (currentChoice != null)
        {
            currentChoice.deselect();
            currentChoice = null;
        }
        if (currentSpace != null)
        {
            currentSpace.deselect();
            currentSpace = null;
        }
    }

    public void checkWin()
    {
        if(!rowCheck())
            return;
        if(!colCheck())
            return;
        if(!quadCheck())
            return;
        Debug.Log("A win was detected!");
        winScreen.SetActive(true);
        this.playWinSound();
        this.gameObject.SetActive(false);
        if (rewardItem != null)
            InventoryManager.addItem(rewardItem);
    }

    bool rowCheck()
    {
        for (int i = 0; i < gridHeight; i++)
        {
            //Sets do not allow duplicate values. Check length after iterating through row.
            HashSet<int> numbers = new HashSet<int>();
            for (int j = 0; j < gridWidth; j++)
            {
                //Check for empty space
                if (grid[j, i].value == 0)
                    return false;
                numbers.Add(grid[j, i].value);
            }
            if (numbers.Count != gridWidth)
                return false;
            Debug.Log("Row check " + i + " was successful!");
        }
        return true;
    }

    bool colCheck()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            //Sets do not allow duplicate values. Check length after iterating through row.
            HashSet<int> numbers = new HashSet<int>();
            for (int j = 0; j < gridHeight; j++)
            {
                //Check for empty space
                if (grid[i, j].value == 0)
                    return false;
                numbers.Add(grid[i, j].value);
            }
            if (numbers.Count != gridHeight)
                return false;
            Debug.Log("Col check " + i + " was successful!");
        }
        return true;
    }

    //There's probably a more elegant way to do this, but this will have to do
    bool quadCheck()
    {
        HashSet<int> numbers;
        //Quadrant 1
        numbers = new HashSet<int>();
        numbers.Add(grid[0, 0].value);
        numbers.Add(grid[1, 0].value);
        numbers.Add(grid[0, 1].value);
        numbers.Add(grid[1, 1].value);
        if (numbers.Count != gridHeight)
            return false;
        Debug.Log("Quad check 0 was successful!");

        //Quadrant 2
        numbers = new HashSet<int>();
        numbers.Add(grid[2, 0].value);
        numbers.Add(grid[3, 0].value);
        numbers.Add(grid[2, 1].value);
        numbers.Add(grid[3, 1].value);
        if (numbers.Count != gridHeight)
            return false;
        Debug.Log("Quad check 1 was successful!");
        
        //Quadrant 3
        numbers = new HashSet<int>();
        numbers.Add(grid[0, 2].value);
        numbers.Add(grid[1, 2].value);
        numbers.Add(grid[0, 3].value);
        numbers.Add(grid[1, 3].value);
        if (numbers.Count != gridHeight)
            return false;
        Debug.Log("Quad check 2 was successful!");

        //Quadrant 4
        numbers = new HashSet<int>();
        numbers.Add(grid[2, 2].value);
        numbers.Add(grid[3, 2].value);
        numbers.Add(grid[2, 3].value);
        numbers.Add(grid[3, 3].value);
        if (numbers.Count != gridHeight)
            return false;
        Debug.Log("Quad check 3 was successful!");

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

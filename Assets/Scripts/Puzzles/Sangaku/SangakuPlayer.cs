using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CoreCLR.NCalc;
 
public class SangakuPlayer : TerminalPuzzle
{
    [SerializeField] GameObject sangakuMenu;
    [SerializeField] TMP_InputField squareInput;
    [SerializeField] TMP_InputField triangleInput;

    [SerializeField] float squareAnswer;
    [SerializeField] float triangleAnswer;
    [SerializeField] float errorMargin;

        //Win screen display
    [SerializeField] private GameObject winScreen;

    //Answers
    //square: Sqrt(3) + 1
    //triangle: (Sqrt(6)+4*Sqrt(3))/3

    //Reward
    [SerializeField] InventoryItem rewardItem;

    void Start()
    {
        powerOff();
    }
    
    public void openMenu()
    {
        this.playSelectSound();
        GamestateManager.setState(GamestateManager.Gamestate.inMenu);
        sangakuMenu.SetActive(true);
    }

    public void closeMenu()
    {
        this.playCancelSound();
        GamestateManager.setState(GamestateManager.Gamestate.firstPerson);
        sangakuMenu.SetActive(false);
    }

    public void submit()
    {
        // Debug.Log("Attempted submit");
        Expression squareExpr;
        try
        {
            squareExpr = new Expression(squareInput.text);
        }
        catch
        {
            squareExpr = new Expression("0");
        }
        Expression triangleExpr;
        try
        {
            triangleExpr = new Expression(triangleInput.text);
        }
        catch
        {
            triangleExpr = new Expression("0");
        }
        
        object squareExprResult = squareExpr.Evaluate();
        float squareExprFloat = System.Convert.ToSingle(squareExprResult);
        
        object triangleExprResult = triangleExpr.Evaluate();
        float triangleExprFloat = System.Convert.ToSingle(triangleExprResult);
        

        if ((squareExprFloat > squareAnswer - errorMargin) && (squareExprFloat < squareAnswer + errorMargin))
        {
            Debug.Log("Correct Square Answer!");
        }

        if ((triangleExprFloat > triangleAnswer - errorMargin) && (triangleExprFloat < triangleAnswer + errorMargin))
        {
            Debug.Log("Correct Triangle Answer!");
        }

        if ((squareExprFloat > squareAnswer - errorMargin) && (squareExprFloat < squareAnswer + errorMargin) && (triangleExprFloat > triangleAnswer - errorMargin) && (triangleExprFloat < triangleAnswer + errorMargin))
        {
            Debug.Log("A win was detected!");
            this.playWinSound();
            winScreen.SetActive(true);
            this.gameObject.SetActive(false);
            if (rewardItem != null)
                InventoryManager.addItem(rewardItem);
            
            //Close Menu without additional sound
            GamestateManager.setState(GamestateManager.Gamestate.firstPerson);
            sangakuMenu.SetActive(false);
        }
        else
        {
            this.playCancelSound();
        }
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

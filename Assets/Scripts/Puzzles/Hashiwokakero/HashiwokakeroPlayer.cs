using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashiwokakeroPlayer : TerminalPuzzle
{
    //Level Data
    public HashiwokakeroLevelSO level;
    
    //Island button prefab - generated on load
    public HashiwokakeroIsland islandPrefab;

    //Bridge prefab - generated when two islands are connected
    public HashiwokakeroBridgeDisplay bridgePrefab;

    //List of islands and bridges on board
    public List<HashiwokakeroIsland> islandList = new List<HashiwokakeroIsland>();
    public List<HashiwokakeroBridge> bridgeList = new List<HashiwokakeroBridge>();
    
    //Currently selected island
    public HashiwokakeroIsland selectedIsland;

    //Win screen display
    [SerializeField] private GameObject winScreen;

    //Debug
    [SerializeField] int numBridges = 0;


    //Reward
    [SerializeField] InventoryItem rewardItem;
    
    //Array of islands
    // Start is called before the first frame update
    void Start()
    {
        loadBoard();
        powerOff();
    }

    //Initialize the board state
    void loadBoard()
    {
        for (int i = 0; i < 7; i++)
        {
            // Debug.Log("HashiwokakeroPlayer: Generating row " + i + ".");
            for (int j = 0; j < 7; j++)
            {
                // Debug.Log("HashiwokakeroPlayer: Generating " + i + ", " + j + ".");
                int islandValue = level.board.getValueAt(i, j);

                if (islandValue > 0) {
                    //Create island
                    HashiwokakeroIsland newIsland = Instantiate<HashiwokakeroIsland>(islandPrefab, interfaceRoot);
                    newIsland.name = "Island " + i + ", " + j;

                    //Set game logic variables
                    newIsland.setPlayer(this);
                    newIsland.setRequiredBridges(islandValue);
                    newIsland.setPosition(i, j);
                    
                    //Move into place on terminal screen
                    Vector3 newIslandPos = newIsland.transform.localPosition;
                    newIslandPos.x = -360 + (i * 120);
                    newIslandPos.y = 360 - (j * 120);
                    newIsland.transform.localPosition = newIslandPos;

                    //Add new island to list
                    islandList.Add(newIsland);
                }
            }
        }
        Debug.Log("Hashiwokakero game started with " + islandList.Count + " islands.");
    }

    //Island clicked
    public void islandClicked(HashiwokakeroIsland clickedIsland)
    {
        //If no islands were selected before button click, select the clicked island
        if (selectedIsland == null)
        {
            selectedIsland = clickedIsland;
            selectedIsland.select();
            this.playSelectSound();
            Debug.Log("Island " + clickedIsland.x + ", " + clickedIsland.y + " was selected.");
            return;
        }
        
        //If an island is selected, check if it can connect to the second clicked island
        //(However, first check if the same island was clicked)
        if (clickedIsland == selectedIsland)
        {
            selectedIsland.deselect();
            selectedIsland = null;
            this.playCancelSound();
            Debug.Log(clickedIsland.name + " was de-selected.");
            return;
        }

        //Are the bridges already connected? If so, cycle bridge type
        HashiwokakeroBridge existingBridge = selectedIsland.getBridgeTo(clickedIsland);
        if (existingBridge != null)
        {
            Debug.Log(clickedIsland.name + " and " + selectedIsland.name + " are already connected, cycling bridge type.");
            existingBridge.cycleNumber();
            selectedIsland.deselect();
            selectedIsland = null;
            checkBoardState();
            this.playSelectSound();
            return;
        }

        //Compare the positions of the two islands. Create new bridge if they pass all checks.

        //Are the islands on the same x or y coordinate?
        HashiwokakeroBridge.orientation direction;
        if (selectedIsland.x == clickedIsland.x)
        {
            direction = HashiwokakeroBridge.orientation.VERTICAL;
        }
        else if (selectedIsland.y == clickedIsland.y)
        {
            direction = HashiwokakeroBridge.orientation.HORIZONTAL;
        }
        else
        {
            Debug.Log("Selections were not aligned. " + clickedIsland.name + " and " + selectedIsland.name + " de-selected.");
            selectedIsland.deselect();
            selectedIsland = null;
            this.playCancelSound();
            return;
        }

        //Check for any obstructing islands
        if(islandObstructionCheck(selectedIsland, clickedIsland, direction))
        {
            Debug.Log("Island obstruction detected. " + clickedIsland.name + " and " + selectedIsland.name + " de-selected.");
            selectedIsland.deselect();
            selectedIsland = null;
            this.playCancelSound();
            return;
        }

        //Check for any obstructing bridges.
        for (int i = 0; i < bridgeList.Count; i++)
        {
            if (horizontalObstructionCheck(selectedIsland, clickedIsland, bridgeList[i]) || 
                verticalObstructionCheck(selectedIsland, clickedIsland, bridgeList[i]))
            {
                Debug.Log("Bridge obstruction detected. " + clickedIsland.name + " and " + selectedIsland.name + " de-selected.");
                selectedIsland.deselect();
                selectedIsland = null;
                this.playCancelSound();
                return;
            }
        }

        //Every check has passed, and bridges are not already connected. Create a new bridge!
        Debug.Log("Creating bridge between " + selectedIsland.name + " and " + clickedIsland.name);
        
        HashiwokakeroBridge newBridge = new HashiwokakeroBridge(selectedIsland, clickedIsland);
        HashiwokakeroBridgeDisplay newDisplayBridge = Instantiate<HashiwokakeroBridgeDisplay>(bridgePrefab, interfaceRoot);
        newDisplayBridge.setTransforms(newBridge);
        newBridge.setDisplay(newDisplayBridge);
        newBridge.setPlayer(this);

        bridgeList.Add(newBridge);
        numBridges = bridgeList.Count;
        selectedIsland.addBridge(newBridge);
        clickedIsland.addBridge(newBridge);

        checkBoardState();

        
        selectedIsland.deselect();
        this.playSelectSound();
        selectedIsland = null;
    }

    bool islandObstructionCheck(HashiwokakeroIsland startIsland, HashiwokakeroIsland endIsland, HashiwokakeroBridge.orientation direction)
    {
        for (int i = 0; i < islandList.Count; i++)
        {
            if ((startIsland == islandList[i]) || (endIsland == islandList[i]))
            {
                //Skip if current island is one of the two chosen endpoints;
                //An island cannot obstruct its own bridge
                continue;
            }

            switch (direction)
            {
                case (HashiwokakeroBridge.orientation.VERTICAL):
                    int lowY  = startIsland.y;
                    int highY = endIsland.y;
                    if (lowY > highY)
                    {
                        int temp = lowY;
                        lowY = highY;
                        highY = temp;
                    }
                    if ((islandList[i].x == startIsland.x) &&
                        (islandList[i].y > lowY) &&
                        (islandList[i].y < highY))
                    {
                        //Obstruction found
                        // Debug.Log("Island obstruction at " + islandList);
                        return true;
                    }
                    break;
                case (HashiwokakeroBridge.orientation.HORIZONTAL):
                    int lowX  = startIsland.x;
                    int highX = endIsland.x;
                    if (lowX > highX)
                    {
                        int temp = lowX;
                        lowX = highX;
                        highX = temp;
                    }
                    if ((islandList[i].y == startIsland.y) &&
                        (islandList[i].x > lowX) &&
                        (islandList[i].x < highX))
                    {
                        //Obstruction found
                        return true;
                    }
                    break;
            }
        }
        
        Debug.Log("Island obstruction test passed!");
        return false;
    }
    

    bool horizontalObstructionCheck(HashiwokakeroIsland startIsland, HashiwokakeroIsland endIsland, HashiwokakeroBridge obstructingBridge)
    {
        //If obstructing and new bridge are parallel, obstruction is not possible
        if (startIsland.x == endIsland.x)
        {
            Debug.Log(startIsland.name + " and " + endIsland.name + " are on the same x-coordinate.");
            return false;
        }
        
        int xIntercept  = obstructingBridge.island1.x;
        int lowY        = obstructingBridge.island1.y;
        int highY       = obstructingBridge.island2.y;
        int yIntercept  = startIsland.y;
        int lowX        = startIsland.x;
        int highX       = endIsland.x;
        // Debug.Log("Attempted bridge spans from " + startIsland.name + " to " + endIsland.name);
        // Debug.Log("Obstructing bridge spans from " + obstructingBridge.island1.name + " to " + obstructingBridge.island2.name);
        // Debug.Log("Intercept at " + xIntercept + ", " + yIntercept);
        if (lowX > highX)
        {
            //Guaranteeing lowX < highX simplifies later logic
            int tempX = lowX;
            lowX = highX;
            highX = tempX;
        }
        if (lowY > highY)
        {
            //Guaranteeing lowY < highY simplifies later logic
            int tempY = lowY;
            lowY = highY;
            highY = tempY;
        }

        //Check if obstructing bridge falls between the start and end islands
        if (xIntercept < lowX)
        {
            //If not, skip this bridge
            // Debug.Log("NON-OBSTRUCTION: xIntercept was found to be lower than lowX");
            return false;
        }
        else if (xIntercept > highX)
        {
            // Debug.Log("NON-OBSTRUCTION: xIntercept was found to be higher than highX");
            return false;
        }

        //Check if obstructing bridge includes the Y level of the new bridge
        if ((yIntercept > lowY) && (yIntercept < highY))
        {
            //If so, bridge is obstructed
            // Debug.Log("Bridge is obstructed");
            return true;
        }
        
        // Debug.Log("Bridge appears to be unobstructed");
        return false;
    }

    bool verticalObstructionCheck(HashiwokakeroIsland startIsland, HashiwokakeroIsland endIsland, HashiwokakeroBridge obstructingBridge)
    {
        //If obstructing and new bridge are parallel, obstruction is not possible
        if (startIsland.y == endIsland.y)
        {
            Debug.Log(startIsland.name + " and " + endIsland.name + " are on the same x-coordinate.");
            return false;
        }
        
        int yIntercept  = obstructingBridge.island1.y;
        int lowX        = obstructingBridge.island1.x;
        int highX       = obstructingBridge.island2.x;
        int xIntercept  = startIsland.x;
        int lowY        = startIsland.y;
        int highY       = endIsland.y;
        // Debug.Log("Attempted bridge spans from " + startIsland.name + " to " + endIsland.name);
        // Debug.Log("Obstructing bridge spans from " + obstructingBridge.island1.name + " to " + obstructingBridge.island2.name);
        // Debug.Log("Intercept at " + xIntercept + ", " + yIntercept);
        if (lowY > highY)
        {
            //Guaranteeing lowX < highX simplifies later logic
            int tempY = lowY;
            lowY = highY;
            highY = tempY;
        }
        if (lowX > highX)
        {
            //Guaranteeing lowY < highY simplifies later logic
            int tempX = lowX;
            lowX = highX;
            highX = tempX;
        }

        //Check if obstructing bridge falls between the start and end islands
        if (yIntercept < lowY)
        {
            //If not, skip this bridge
            // Debug.Log("NON-OBSTRUCTION: xIntercept was found to be lower than lowX");
            return false;
        }
        else if (yIntercept > highY)
        {
            // Debug.Log("NON-OBSTRUCTION: xIntercept was found to be higher than highX");
            return false;
        }

        //Check if obstructing bridge includes the Y level of the new bridge
        if ((xIntercept > lowX) && (xIntercept < highX))
        {
            //If so, bridge is obstructed
            // Debug.Log("Bridge is obstructed");
            return true;
        }
        
        // Debug.Log("Bridge appears to be unobstructed");
        return false;
    }

    //Remove a bridge from the list
    public bool removeBridge(HashiwokakeroBridge bridgeToDestroy)
    {
        for(int i = 0; i < bridgeList.Count; i++)
        {
            if (bridgeToDestroy == bridgeList[i])
            {
                Debug.Log("Removed a bridge successfully!");
                bridgeList.RemoveAt(i);
                numBridges = bridgeList.Count;
                checkBoardState();
                return true;
            }
        }
        Debug.Log("Bridge removal failed!");
        return false;
    }

    //Check for a win
    bool checkBoardState()
    {
        for (int i = 0; i < islandList.Count; i++)
        {
            if (!islandList[i].checkNumBridges())
            {
                return false;
            }
        }
        Debug.Log("A win was detected!");
        this.playWinSound();
        winScreen.SetActive(true);
        this.gameObject.SetActive(false);
        if (rewardItem != null)
            InventoryManager.addItem(rewardItem);
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

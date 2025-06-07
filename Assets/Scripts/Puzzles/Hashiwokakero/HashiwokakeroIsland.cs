using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HashiwokakeroIsland : ButtonManager
{
    // enum bridgeType {
    //     NONE,
    //     SINGLE,
    //     DOUBLE
    // }

    //Root game logic
    HashiwokakeroPlayer player;

    //Game logic vars
    public int x;
    public int y;
    int requiredBridges;

    //List of bridges connecting this island
    List<HashiwokakeroBridge> connectingBridges = new List<HashiwokakeroBridge>();

    //Display
    [SerializeField] TMP_Text requiredBridgesText;

    //Setup functions
    public void setPlayer(HashiwokakeroPlayer newPlayer)
    {
        player = newPlayer;
    }
    
    public void setPosition(int xPos, int yPos)
    {
        x = xPos;
        y = yPos;
    }

    public void setRequiredBridges(int numBridges)
    {
        requiredBridges = numBridges;

        //Update display text
        requiredBridgesText.text = requiredBridges.ToString();
    }

    //When player clicks on this island
    public override void onButtonClick()
    {
        // Debug.Log("Island " + x + ", " + y + " received click!");
        player.islandClicked(this);
    }

    //Add a bridge to the list
    public void addBridge(HashiwokakeroBridge newBridge)
    {
        connectingBridges.Add(newBridge);
    }

    //Remove a bridge that has been destroyed
    public bool removeBridge(HashiwokakeroBridge bridgeToDestroy)
    {
        for(int i = 0; i < connectingBridges.Count; i++)
        {
            if (bridgeToDestroy == connectingBridges[i])
            {
                connectingBridges.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    //Check if this bridge is connected to another bridge
    public HashiwokakeroBridge getBridgeTo(HashiwokakeroIsland other)
    {
        foreach (HashiwokakeroBridge b in connectingBridges)
        {
            //Don't need to check for equivalence with this island--
            //every bridge in list should have this island among its entries
            if ((b.island1 == other) || (b.island2 == other))
            {
                return b;
            }
        }

        //If no bridges with connection are found, return null
        return null;
    }

    //Check if island has the correct number of bridges
    public bool checkNumBridges()
    {
        int totalBridges = 0;
        for (int i = 0; i < connectingBridges.Count; i++)
        {
            if (connectingBridges[i].bridgeNumber == HashiwokakeroBridge.number.SINGLE)
            {
                totalBridges++;
            }
            else if (connectingBridges[i].bridgeNumber == HashiwokakeroBridge.number.DOUBLE)
            {
                totalBridges = totalBridges + 2;
            }
        }

        if (totalBridges == requiredBridges)
        {
            return true;
        }
        Debug.Log(this.name + " does not have the correct number of bridges. (Expected: " + requiredBridges + ", Actual: " + connectingBridges.Count);
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashiwokakeroBridge
{
    public HashiwokakeroBridge(HashiwokakeroIsland i1, HashiwokakeroIsland i2)
    {
        island1 = i1;
        island2 = i2;
        bridgeNumber = number.SINGLE;
        
        //Determine the orientation of the bridge
        if (i1.x == i2.x)
        {
            bridgeDirection = orientation.VERTICAL;
        }
        else if (i1.y == i2.y)
        {
            bridgeDirection = orientation.HORIZONTAL;
        }
        else
        {
            Debug.LogError("Bridge was created with invalid island positions. Orientation cannot be vertical or horizontal!");
            bridgeDirection = orientation.INVALID;
        }
    }

    public enum number {
        NONE,
        SINGLE,
        DOUBLE
    }

    public enum orientation {
        HORIZONTAL,
        VERTICAL,
        INVALID
    }

    public number bridgeNumber;
    public orientation bridgeDirection;
    
    //Hashiwokakero player
    public HashiwokakeroPlayer player;

    //What islands is this bridge connected to?
    public HashiwokakeroIsland island1;
    public HashiwokakeroIsland island2;

    //Used for displaying bridge
    public HashiwokakeroBridgeDisplay displayBridge;
    
    //Set the instance of the game
    public void setPlayer(HashiwokakeroPlayer newPlayer)
    {
        player = newPlayer;
    }

    //Set up display bridge
    public void setDisplay(HashiwokakeroBridgeDisplay newDisplayBridge)
    {
        displayBridge = newDisplayBridge;
    }

    //Get orientation/type of bridge
    public number getNumber()
    {
        return bridgeNumber;
    }

    public orientation getDirection()
    {
        return bridgeDirection;
    }

    //Update bridge type if an existing bridge is selected
    public void cycleNumber()
    {
        switch (bridgeNumber)
        {
            case (number.SINGLE):
                //If bridge is singular, change it to double
                bridgeNumber = number.DOUBLE;
                displayBridge.displayDouble();
                break;
            case (number.DOUBLE):
                //If bridge is double, delete it
                destroyBridge();
                break;
            case (number.NONE):
                Debug.LogError("Something went wrong. A bridge with bridgeNumber = NONE was detected! (cycleNumber)");
                break;
        }
    }

    //Bridge removed from board; clean up
    public void destroyBridge()
    {
        //Delete display
        GameObject.Destroy(displayBridge.gameObject);

        //Remove reference to this bridge from islands
        island1.removeBridge(this);
        island2.removeBridge(this);

        //Remove reference from the player
        player.removeBridge(this);
    }
}

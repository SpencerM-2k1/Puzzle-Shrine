using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashiwokakeroBridgeDisplay : MonoBehaviour
{
    //Display objects for bridge modes
    //  Only one group should display at a time
    [SerializeField] GameObject single;
    [SerializeField] GameObject double1;
    [SerializeField] GameObject double2;

    //Debug fields
    //  These should NOT be accessed for any logic. But I need to see what is going wrong.
    public HashiwokakeroIsland island1;
    public HashiwokakeroIsland island2;
    public HashiwokakeroBridge.orientation debugOrientation;

    public void displaySingle()
    {
        single.SetActive(true);
        double1.SetActive(false);
        double2.SetActive(false);
    }

    public void displayDouble()
    {
        single.SetActive(false);
        double1.SetActive(true);
        double2.SetActive(true);
    }

    public void setTransforms(HashiwokakeroBridge bridge)
    {
        //Get island positions for easier comparison
        RectTransform thisTrans = this.GetComponent<RectTransform>();   //Technically unsafe, but this should NEVER be on a non-RectTransform
        Vector3 thisPos = thisTrans.anchoredPosition;
        Vector3 islandPos1 = bridge.island1.GetComponent<RectTransform>().anchoredPosition;
        Vector3 islandPos2 = bridge.island2.GetComponent<RectTransform>().anchoredPosition;

        //Set debug values
        island1 = bridge.island1;
        island2 = bridge.island2;


        int unitDistance = 0;

        switch (bridge.bridgeDirection)
        {
            case (HashiwokakeroBridge.orientation.HORIZONTAL):
                this.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0)); //points right from origin
                if (islandPos1.x < islandPos2.x)
                {
                    thisPos = islandPos1;
                }
                else
                {
                    thisPos = islandPos2;
                }
                // Debug.Log("bridge.island1.x - bridge.island2.x = " + bridge.island1.x + " - " + bridge.island2.x);
                unitDistance = Mathf.Abs(bridge.island1.x - bridge.island2.x);
                // Debug.Log("X distance: " + unitDistance + " spaces");
                break;
            case (HashiwokakeroBridge.orientation.VERTICAL):
                this.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90)); //points down from origin
                if (islandPos1.y > islandPos2.y)
                {
                    thisPos = islandPos1;
                }
                else
                {
                    thisPos = islandPos2;
                }
                // Debug.Log("bridge.island1.y - bridge.island2.y = " + bridge.island1.y + " - " + bridge.island2.y);
                unitDistance = Mathf.Abs(bridge.island1.y - bridge.island2.y);
                // Debug.Log("Y distance: " + unitDistance + " spaces");
                break;
        }
        this.GetComponent<RectTransform>().anchoredPosition = thisPos;
        this.transform.localScale = new Vector3((float)unitDistance, this.transform.localScale.y, this.transform.localScale.z);
    }
}

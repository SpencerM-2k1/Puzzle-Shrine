using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangramPiece : ButtonManager
{
    Vector2 dimensions;
    Vector2 center;
    
    [SerializeField] Vector2 goal;
    [SerializeField] float maxRadius = 0.2f;

    PlayerLook playerLook;
    [SerializeField] GameObject screen;

    TangramPlayer player;

    bool isDragging = false;

    // Start is called before the first frame update
    void Start()
    {
        playerLook = PlayerManager.instance.GetComponentInChildren<PlayerLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            Vector3 hitPos;
            if (playerLook.isLookingAt(screen, LayerMask.GetMask("TerminalScreen"), out hitPos))
            {
                // Debug.Log("hitPos: " + hitPos);
                Vector3 position = this.transform.position;
                position.y = hitPos.y;
                position.z = hitPos.z;
                this.transform.position = position;
            }
            else
            {
                isDragging = false;
            }
        }
    }

    //Set parent game's TangramPlayer
    public void setPlayer(TangramPlayer newPlayer)
    {
        this.player = newPlayer;
    }

    //Check if this piece is within the margin of error
    public bool checkWin()
    {
        //Calculate distance to intended goal
        float xDif = this.transform.localPosition.x - goal.x;
        float yDif = this.transform.localPosition.y - goal.y;
        float distance = Mathf.Sqrt(Mathf.Pow(xDif, 2f) + Mathf.Pow(yDif, 2f));
        Debug.Log("distance: " + distance);

        //Compare with maxRadius
        if (distance < maxRadius)
        {
            Debug.Log("Tangram piece is in range!");
            return true;
        }
        return false;
    }

    //When player clicks on this piece, start dragging on screen
    public override void onButtonClick()
    {
        // Debug.Log("Tangram piece clicked!");
        isDragging = !isDragging;
        if (isDragging)
            player.playSelectSound();
        else
            player.playCancelSound();
        player.checkWinState();
    }
}

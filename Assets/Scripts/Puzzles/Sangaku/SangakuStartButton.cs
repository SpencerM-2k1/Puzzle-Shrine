using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SangakuStartButton : ButtonManager
{
    [SerializeField] SangakuPlayer player;

    //When player clicks on this island
    public override void onButtonClick()
    {
        player.openMenu();
    }
}

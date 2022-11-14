using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGame : BaseState
{

    public PreGame()
    {
        Name = Constants.STATE_PREGAME_ID;
    }

    public override void Enter(GameMode gm)
    {
        gm?.preGame();
    }
}

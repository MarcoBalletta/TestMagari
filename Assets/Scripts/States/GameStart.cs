using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : BaseState
{
    public GameStart()
    {
        Name = Constants.STATE_STARTGAME_ID;
    }

    public override void Enter(GameMode gm)
    {
        gm?.gameStart();
    }

    public override void Exit()
    {
        
    }
}

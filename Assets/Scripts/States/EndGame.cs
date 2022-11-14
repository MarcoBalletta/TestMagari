using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : BaseState
{

    public EndGame()
    {
        Name = Constants.STATE_ENDGAME_ID;
    }
    public override void Enter(GameMode gm)
    {
        gm.endGame();
    }
}

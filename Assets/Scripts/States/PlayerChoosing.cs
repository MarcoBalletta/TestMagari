using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChoosing : BaseState
{

    public PlayerChoosing()
    {
        Name = Constants.STATE_PLAYERCHOOSING_ID;
    }

    public override void Enter(GameMode gm)
    {
        gm.playerChooseStartingTile();
    }
}

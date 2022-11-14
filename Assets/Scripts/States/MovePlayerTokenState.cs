using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerTokenState : BaseState
{

    public MovePlayerTokenState()
    {
        Name = Constants.STATE_MOVEPLAYERTOKEN_ID;
    }
    public override void Enter(GameMode gm)
    {
        gm.movePlayerToken();
    }
}

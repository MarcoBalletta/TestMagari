using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : BaseState
{
    public EndTurn()
    {
        Name = Constants.STATE_ENDTURN_ID;
    }

    public override void Enter(GameMode gm)
    {
        gm.endTurn();
    }

}

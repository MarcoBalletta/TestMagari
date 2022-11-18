using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickCardState : BaseState
{

    public PickCardState()
    {
        Name = Constants.STATE_PICKCARD_ID;
    }
    public override void Enter(GameMode gm)
    {
        gm.pickCard();
    }
}

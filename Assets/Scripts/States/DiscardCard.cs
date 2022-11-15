using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardCard : BaseState
{
    public DiscardCard()
    {
        Name = Constants.STATE_DISCARDCARD_ID;
    }

    public override void Enter(GameMode gm)
    {
        gm.discardCard();
    }
}

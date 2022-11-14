using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardCard : BaseState
{
    public DiscardCard()
    {
        Name = Constants.INFO_DISCARD_CARD;
    }

    public override void Enter(GameMode gm)
    {
        gm.discardCard();
    }
}

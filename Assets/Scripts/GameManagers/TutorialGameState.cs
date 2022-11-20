using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameState : GameState
{
    public override void Setup(Board board)
    {
        boardInGame = board;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameMode : GameMode
{

    public override void Awake()
    {
        base.Awake();
        gameStart += TutorialSpawnStartingEndingTile;
    }

    protected override void Start()
    {
        stateManager.ChangeState(Constants.STATE_PREGAME_ID, this);
        stateManager.ChangeState(Constants.STATE_STARTGAME_ID, this);
    }

    private void TutorialSpawnStartingEndingTile()
    {
        gameState.PlayersInGame[0].StartingTile = gameState.BoardInGame.MapTiles[new Vector2Int(0, 0)];
        gameState.PlayersInGame[0].EndingTile = gameState.BoardInGame.MapTiles[new Vector2Int(0, 2)];
    }

}

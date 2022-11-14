using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveUnitCommand : BaseCommand
{

    private int row;
    private int column;
    private GameMode gm;

    public MoveUnitCommand(int rowNew, int columnNew, GameMode gameMode)
    {
        row = rowNew;
        column = columnNew;
        gm = gameMode;
    }

    public override void Execute()
    {
        gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].GetComponent<NavMeshAgent>().SetDestination(gm.GameState.BoardInGame.MapTiles[new Vector2Int(row, column)].transform.position);
    }
}

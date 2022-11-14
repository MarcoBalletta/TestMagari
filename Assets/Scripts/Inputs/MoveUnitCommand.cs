using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveUnitCommand : BaseCommand
{

    private int row;
    private int column;
    private Board board;
    private PlayerManager player;

    public MoveUnitCommand(int rowNew, int columnNew, PlayerManager playerToMove, Board boardNew)
    {
        row = rowNew;
        column = columnNew;
        player = playerToMove;
        board = boardNew;
    }

    public override void Execute()
    {
        player.GetComponent<NavMeshAgent>().destination = board.MapTiles[new Vector2Int(row, column)].transform.position;
        player.playerMoving();
    }
}

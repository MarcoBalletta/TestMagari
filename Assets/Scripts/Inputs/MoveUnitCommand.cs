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

    public MoveUnitCommand(int rowNew, int columnNew, PlayerManager playerToMove, Board boardNew, Directions comingFromDirection)
    {
        row = rowNew;
        column = columnNew;
        player = playerToMove;
        board = boardNew;
        switch (comingFromDirection)
        {
            case Directions.north:
                player.ComingFromDirection = Directions.south;
                break;
            case Directions.south:
                player.ComingFromDirection = Directions.north;
                break;
            case Directions.west:
                player.ComingFromDirection = Directions.east;
                break;
            case Directions.east:
                player.ComingFromDirection = Directions.west;
                break;
        }
    }

    public override void Execute()
    {
        player.GetComponent<NavMeshAgent>().destination = board.MapTiles[new Vector2Int(row, column)].transform.position;
        player.playerMoving(board.MapTiles[new Vector2Int(row, column)]);
    }
}

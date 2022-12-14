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
        Vector3 position = Vector3.zero;
        if (board.MapTiles[new Vector2Int(row, column)].Data.CardTile != null)
        {
            position = board.MapTiles[new Vector2Int(row, column)].transform.position + GiveDestinationFromDirection(player.ComingFromDirection, board.MapTiles[new Vector2Int(row, column)].Data.CardTile);
            //Debug.Log(position);
        }
        else 
        {
            position = board.MapTiles[new Vector2Int(row, column)].transform.position + new Vector3(0, 0.25f, 0);
            //Debug.Log(position);
        }
        bool result = player.Agent.SetDestination(new Vector3(position.x, position.y + 0.25f, position.z));
        //Debug.Log("Player position : " + player.transform.position + "destination: " + player.Agent.destination + "bool " + result);
        player.playerMoving(board.MapTiles[new Vector2Int(row, column)]);
        board.BakeArea();
    }

    private Vector3 GiveDestinationFromDirection(Directions direction, Card card)
    {
        foreach(var dir in card.CorridorDirections)
        {
            if (dir.Direction.HasFlag(direction))
            {
                return dir.PlayerPosition;
            }
        }
        return Vector3.zero;
    }
}

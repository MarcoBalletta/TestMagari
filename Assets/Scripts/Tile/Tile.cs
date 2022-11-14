using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private GameMode gm;
    private Board board;
    private TileData data;
    private PlayerManager playerOnTile;

    public TileData Data { get => data; }
    public PlayerManager PlayerOnTile { get => playerOnTile; set => playerOnTile = value; }

    public void Setup(GameMode gameMode, int rowNew, int columnNew, TileType newType)
    {
        gm = gameMode;
        board = gm.GameState.BoardInGame;
        data = new TileData(rowNew, columnNew, newType);
    }

    public void SetupCard(Card newCard)
    {
        data.CardTile = newCard; 
    }

    private void OnMouseDown()
    {
        if (gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].StartingTile == null && gm.StateManager.Current.Name == Constants.STATE_PLAYERCHOOSING_ID && data.Type == TileType.empty)
        {
            gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].StartingTile = this;
            data.Type = TileType.starting;
            gm.StateManager.ChangeState(Constants.STATE_PLAYERCHOOSING_ID, gm);
        }else if(CheckPickCardConditions())
        {
            board.CheckIfTileCanBeSpawned(data.Row, data.Column, gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].CardSelected);
        }else if (CheckMoveTokenConditions())
        {

        }
    }

    private void OnMouseEnter()
    {
        if(transform.localScale == Vector3.one && (CheckPickCardConditions() || CheckMoveTokenConditions()))
        {
            transform.localScale *= 0.5f;
            GetComponent<BoxCollider>().size = Vector3.one * 2;
        }
    }

    private void OnMouseExit()
    {
        if (transform.localScale == Vector3.one * 0.5f && (CheckPickCardConditions() || CheckMoveTokenConditions()))
        {
            transform.localScale *= 2f;
            GetComponent<BoxCollider>().size = Vector3.one;
        }
    }

    private bool CheckPickCardConditions()
    {
        if (gm.StateManager.Current.Name == Constants.STATE_PICKCARD_ID && board.PositionableTileConditions(data.Row, data.Column) && gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].CardSelected != null) return true;
        else return false;
    }

    private bool CheckMoveTokenConditions()
    {
        if (gm.StateManager.Current.Name == Constants.STATE_MOVEPLAYERTOKEN_ID && data.Type != TileType.empty && IsAdjacent(gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].ActualTile.Data) && board.CheckTilesConnected(gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].ActualTile.Data, data)) return true;
        else return false;
    }

    private bool IsAdjacent(TileData tileToConfront)
    {
        if ((data.Row == tileToConfront.Row && Mathf.Abs(data.Column - tileToConfront.Column) == 1) || (data.Column == tileToConfront.Column && Mathf.Abs(data.Row - tileToConfront.Row) == 1)) return true;
        else return false;
    }
}

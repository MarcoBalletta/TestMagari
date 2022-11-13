using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private GameMode gm;
    private TileData data;
    private PlayerManager playerOnTile;

    public TileData Data { get => data; }
    public PlayerManager PlayerOnTile { get => playerOnTile; set => playerOnTile = value; }

    public void Setup(GameMode gameMode, int rowNew, int columnNew, TileType newType)
    {
        gm = gameMode;
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
        }else if(gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].CardSelected != null && gm.StateManager.Current.Name == Constants.STATE_PICKCARD_ID && gm.GameState.BoardInGame.PositionableTileConditions(data.Row, data.Column))
        {
            Debug.Log(" check if possibile and spawn card in place");
            gm.GameState.BoardInGame.CheckIfTileCanBeSpawned(data.Row, data.Column, gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].CardSelected);
        }
    }

    private void OnMouseEnter()
    {
        if(gm.StateManager.Current.Name == Constants.STATE_PICKCARD_ID && gm.GameState.BoardInGame.PositionableTileConditions(data.Row, data.Column) && gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].CardSelected != null)
        {
            transform.localScale *= 0.5f;
            GetComponent<BoxCollider>().size = Vector3.one * 2;
        }
    }

    private void OnMouseExit()
    {
        if (gm.StateManager.Current.Name == Constants.STATE_PICKCARD_ID && gm.GameState.BoardInGame.PositionableTileConditions(data.Row, data.Column) && gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].CardSelected != null)
        {
            transform.localScale *= 2f;
            GetComponent<BoxCollider>().size = Vector3.one;
        }
    }
}

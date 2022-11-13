using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    private int indexTurn;
    private Tile startingTile;
    private Tile endingTile;
    private Tile actualTile;
    private GameMode gm;
    private Board board;
    private Card[] playerCards;
    private Card cardSelected;
    [SerializeField] private Tile prefabStartingTile;
    [SerializeField] private Tile prefabEndingTile;
    [SerializeField] private GameObject model;
    [SerializeField] private Animator anim;

    public void Init(int indexList, GameMode gameMode, Board boardToPass)
    {
        indexTurn = indexList;
        gm = gameMode;
        gm.gameStart += SpawnStartingEndingTiles;
        gm.gameStart += ShowModel;
        board = boardToPass;
    }

    private void SpawnStartingEndingTiles()
    {
        //spawn starting tile and adjust data
        var starting = Instantiate(prefabStartingTile, board.transform);
        starting.Setup(gm, startingTile.Data.Row, startingTile.Data.Column, TileType.starting);
        starting.transform.position = startingTile.transform.position;
        Destroy(startingTile.gameObject);
        startingTile = starting;
        board.MapTiles[new Vector2Int(startingTile.Data.Row, startingTile.Data.Column)] = startingTile;
        transform.position = startingTile.transform.position;

        //spawn ending tile and adjust data
        var ending = Instantiate(prefabEndingTile, board.transform);
        ending.Setup(gm, endingTile.Data.Row, endingTile.Data.Column, TileType.ending);
        ending.transform.position = endingTile.transform.position;
        Destroy(endingTile.gameObject);
        endingTile = ending;
        board.MapTiles[new Vector2Int(ending.Data.Row, ending.Data.Column)] = ending;
    }

    private void ShowModel()
    {
        model.SetActive(true);
    }

    public void SwapTilesDrawCard(Card card)
    {
        foreach(var cardInList in playerCards)
        {
            if(cardInList == null)
            {
                playerCards[Array.IndexOf(playerCards, cardInList)] = card;
                break;
            }
        }
        gm.updateUICards();
    }

    public void DiscardCard(Card cardDiscarded)
    {
        var index = Array.IndexOf(playerCards, cardDiscarded);
        if (index == -1) return;
        playerCards[index] = null;
        gm.updateUICards();
    }

    public int IndexTurn { get => indexTurn; set => indexTurn = value; }
    public Tile StartingTile { get => startingTile; set => startingTile = value; }
    public Tile EndingTile { get => endingTile; set => endingTile = value; }
    public Card[] PlayerCards { get => playerCards; set => playerCards = value; }
    public Tile ActualTile { get => actualTile; set => actualTile = value; }
    public Card CardSelected { get => cardSelected; set => cardSelected = value; }
}

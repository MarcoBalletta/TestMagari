using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    private int indexTurn;
    private Tile startingTile;
    private Tile endingTile;
    private Tile actualTile;
    private Directions comingFromDirection = Directions.none;
    private GameMode gm;
    private Board board;
    private Card[] playerCards;
    private Card cardSelected;
    private NavMeshAgent agent;
    private Coroutine movementCoroutine;
    [SerializeField] private Tile prefabStartingTile;
    [SerializeField] private Tile prefabEndingTile;
    [SerializeField] private GameObject model;
    [SerializeField] private AnimAndVFXHandler animHandler;
    public delegate void PlayerMoving(Tile data);
    public PlayerMoving playerMoving;

    public void Init(int indexList, GameMode gameMode, Board boardToPass)
    {
        indexTurn = indexList;
        gm = gameMode;
        gm.gameStart += SpawnStartingEndingTiles;
        gm.gameStart += ShowModel;
        board = boardToPass;
        agent = GetComponent<NavMeshAgent>();
        playerMoving += CheckIfArrivedAtDestination;
        playerMoving += StartRunning;
    }

    private void SpawnStartingEndingTiles()
    {
        //spawn starting tile and adjust data
        var starting = Instantiate(prefabStartingTile, board.transform);
        //board.BakeArea();
        starting.Setup(gm, startingTile.Data.Row, startingTile.Data.Column, TileType.starting, false);
        starting.transform.position = startingTile.transform.position;
        Destroy(startingTile.gameObject);
        startingTile = starting;
        startingTile.PlayerOnTile = this;
        actualTile = startingTile;
        board.MapTiles[new Vector2Int(startingTile.Data.Row, startingTile.Data.Column)] = startingTile;
        //Debug.Log("position: " + transform.position + "destination: " + agent.destination);
        transform.position = startingTile.transform.position + new Vector3(0, 0.25f, 0);
        //var result = agent.SetDestination(startingTile.transform.position + new Vector3(0, 0.25f, 0));
        //Debug.Log("New position: " + transform.position + "destination attempting:  " + (startingTile.transform.position + new Vector3(0, 0.25f, 0)) + " destination: " + agent.destination);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().Sleep();

        //spawn ending tile and adjust data
        var ending = Instantiate(prefabEndingTile, board.transform);
        ending.Setup(gm, endingTile.Data.Row, endingTile.Data.Column, TileType.ending, false);
        ending.transform.position = endingTile.transform.position;
        Destroy(endingTile.gameObject);
        endingTile = ending;
        board.MapTiles[new Vector2Int(ending.Data.Row, ending.Data.Column)] = ending;
        agent.enabled = true;
        board.BakeArea();
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
        if(gm.StateManager.Current.Name == Constants.STATE_DISCARDCARD_ID)  gm.StateManager.ChangeState(Constants.STATE_ENDTURN_ID, gm);
    }



    private void CheckIfArrivedAtDestination(Tile data)
    {
        if(movementCoroutine == null) movementCoroutine = StartCoroutine(ControlIfArrived(data));
    }

    private IEnumerator ControlIfArrived(Tile data)
    {
        actualTile.PlayerOnTile = null;
        yield return new WaitForSeconds(0.2f);
        while (movementCoroutine != null && Vector3.Distance(transform.position, new Vector3(agent.destination.x, transform.position.y, agent.destination.z)) > agent.stoppingDistance)
        {
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine(movementCoroutine);
        animHandler.StopRunning();
        agent.SetDestination(transform.position);
        movementCoroutine = null;
        actualTile = data;
        data.PlayerOnTile = this;
        if (actualTile == endingTile) gm.StateManager.ChangeState(Constants.STATE_ENDGAME_ID, gm);
        else if(gm.GameState.PlayersInGame[gm.GameState.PlayerTurn] == this) 
        {
            gm.IncreaseSteps();
            if (!gm.CanMoveAgain() || actualTile.Data.IsTrap) gm.playerMoved();
        } 
    }
    
    private void StartRunning(Tile data)
    {
        animHandler.StartRunning();
    }


    public void CheckNumberOfCards()
    {
        //if (indexTurn != gm.GameState.PlayerTurn) return;
        var arrayCards = playerCards.Where(card => card != null);
        if(arrayCards.Count() > Constants.MAX_NUMBER_OF_CARDS)
        {
            gm.StateManager.ChangeState(Constants.STATE_DISCARDCARD_ID, gm);
        }
        else
        {
            gm.StateManager.ChangeState(Constants.STATE_ENDTURN_ID, gm);
        }
    }

    public int IndexTurn { get => indexTurn; set => indexTurn = value; }
    public Tile StartingTile { get => startingTile; set => startingTile = value; }
    public Tile EndingTile { get => endingTile; set => endingTile = value; }
    public Card[] PlayerCards { get => playerCards; set => playerCards = value; }
    public Tile ActualTile { get => actualTile; set => actualTile = value; }
    public Card CardSelected { get => cardSelected; set => cardSelected = value; }
    public Directions ComingFromDirection { get => comingFromDirection; set => comingFromDirection = value; }
    public NavMeshAgent Agent { get => agent; set => agent = value; }
}

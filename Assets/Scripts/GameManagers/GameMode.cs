using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    private GameState gameState;
    private StateManager stateManager;
    public delegate void PreGameStart();
    public PreGameStart preGame;
    public delegate void PlayerChooseStartingTile();
    public PlayerChooseStartingTile playerChooseStartingTile;
    public delegate void GameStart();
    public GameStart gameStart;
    public delegate void PickCardStart();
    public PickCardStart pickCard;
    public delegate void PickedCardStart();
    public PickedCardStart pickedCard;
    public delegate void UpdateUICards();
    public PickedCardStart updateUICards;
    public delegate void MovePlayerTokenStart();
    public MovePlayerTokenStart movePlayerToken;
    public delegate void EndGameStart();
    public EndGameStart endGame;
    public GameState GameState { get => gameState; }
    public StateManager StateManager { get => stateManager; }


    // Start is called before the first frame update
    void Awake()
    {
        gameState = GetComponent<GameState>();
        stateManager = GetComponent<StateManager>();
        GameState.BoardInGame = Instantiate(gameState.BoardRef, Vector3.zero, Quaternion.identity);
        GameState.BoardInGame.Init(this);
        preGame += CreatePlayers;
        playerChooseStartingTile += CheckForOtherPlayersToSelectStartingTile;
        pickCard += IncreaseTurn;
        gameStart += ChooseStartingPlayer;
    }

    private void Start()
    {
        stateManager.ChangeState(Constants.STATE_PREGAME_ID, this);
        stateManager.ChangeState(Constants.STATE_PLAYERCHOOSING_ID, this);
    }

    private void ChooseStartingPlayer()
    {
        gameState.PlayerTurn = Random.Range(0, gameState.PlayersPrefab.Count);
        stateManager.ChangeState(Constants.STATE_PICKCARD_ID, this);
    }

    private void CreatePlayers()
    {
        foreach(var player in gameState.PlayersPrefab)
        {
            var pl = (Instantiate(player));
            pl.Init(gameState.PlayersInGame.Count, this, gameState.BoardInGame);
            gameState.PlayersInGame.Add(pl);
        }
    }

    public void CheckForOtherPlayersToSelectStartingTile()
    {
        foreach(var player in gameState.PlayersInGame)
        {
            if(player.StartingTile == null)
            {
                gameState.PlayerTurn = gameState.PlayersInGame.IndexOf(player);
                return;
            }
        }
        foreach(var player in gameState.PlayersInGame)
        {
            gameState.BoardInGame.ChooseEndingTile(player);
        }
        //start game
        stateManager.ChangeState(Constants.STATE_STARTGAME_ID, this);
    }

    private void IncreaseTurn()
    {
        gameState.TotalTurns++;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public delegate void PlayerMoved();
    public PlayerMoved playerMoved;
    //public delegate void FinishedPlayerMoving();
    //public FinishedPlayerMoving finishedPlayerMoving;
    public delegate void DiscardCard();
    public DiscardCard discardCard;
    public delegate void EndTurn();
    public EndTurn endTurn;
    public delegate void EndGameStart();
    public EndGameStart endGame;

    public delegate void EnableMainCamera();
    public EnableMainCamera enableMainCamera;
    public delegate void EnableTableCamera();
    public EnableTableCamera enableTableCamera;
    public GameState GameState { get => gameState; }
    public StateManager StateManager { get => stateManager; }


    // Start is called before the first frame update
    void Awake()
    {
        gameState = GetComponent<GameState>();
        stateManager = GetComponent<StateManager>();
        var board = Instantiate(gameState.BoardRef, Vector3.zero, Quaternion.identity);
        gameState.Setup(board);
        board.Init(this);
        preGame += CreatePlayers;
        playerChooseStartingTile += CheckForOtherPlayersToSelectStartingTile;
        pickCard += ResetCardsPlayed;
        pickCard += ResetSteps;
        pickCard += IncreaseTurn;
        playerMoved += ControlPlayersCards;
        gameStart += ChooseStartingPlayer;
        endTurn += EndTurnPlayer;
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

    private void EndTurnPlayer()
    {
        IncreasePlayerturn();
        stateManager.ChangeState(Constants.STATE_PICKCARD_ID, this);
    }

    private void IncreasePlayerturn()
    {
        gameState.PlayerTurn++;
        if(gameState.PlayerTurn >= gameState.PlayersInGame.Count)
        {
            gameState.PlayerTurn = 0;
        }
    }

    public void SkipPhase()
    {
        if(stateManager.Current.Name == Constants.STATE_PICKCARD_ID)
        {
            stateManager.ChangeState(Constants.STATE_MOVEPLAYERTOKEN_ID, this);
        }else if(stateManager.Current.Name == Constants.STATE_MOVEPLAYERTOKEN_ID)
        {
            playerMoved();
        }
    }

    private void ControlPlayersCards()
    {
        gameState.PlayersInGame[gameState.PlayerTurn].CheckNumberOfCards();
    }

    private void ResetSteps()
    {
        gameState.StepsInTurn = 0;
    }

    public void IncreaseSteps()
    {
        gameState.StepsInTurn++;
    }

    public bool CanMoveAgain()
    {
        if (gameState.StepsInTurn < Constants.MAXIMUM_TOKEN_STEPS) return true;
        else return false;
    }

    private void ResetCardsPlayed()
    {
        gameState.CardsPlayerdInTurn = 0;
    }

    public void IncreaseCardsPlayed()
    {
        gameState.CardsPlayerdInTurn++;
    }

    public bool CanPlayCardAgain()
    {
        if (gameState.CardsPlayerdInTurn < Constants.MAXIMUM_CARDS_PLAYABLE) return true;
        else return false;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(Constants.MENU_SCENE_NAME );
    }

    public void StartGame()
    {
        SceneManager.LoadScene(Constants.GAME_SCENE_NAME);
    }
}

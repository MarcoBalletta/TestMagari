using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] private Board boardRef;
    [SerializeField] private Board boardInGame;
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private List<PlayerManager> playersPrefab = new List<PlayerManager>();
    [SerializeField] private List<PlayerManager> playersInGame = new List<PlayerManager>();
    private int playerTurn;
    private int totalTurns;
    private int stepsInTurn;
    private int cardsPlayerdInTurn;

    public void Setup(Board board)
    {
        if (GridDataSelection.rows > 0) rows = GridDataSelection.rows;
        else rows = Constants.STANDARD_GRID_ROW_SIZE;
        if (GridDataSelection.columns > 0) columns = GridDataSelection.columns;
        else columns = Constants.STANDARD_GRID_COLUMN_SIZE;
        boardInGame = board;
    }

    public Board BoardRef { get => boardRef; }
    public int Rows { get => rows; }
    public int Columns { get => columns; }
    public List<PlayerManager> PlayersPrefab { get => playersPrefab; }
    public int PlayerTurn { get => playerTurn; set => playerTurn = value; }
    public List<PlayerManager> PlayersInGame { get => playersInGame; set => playersInGame = value; }
    public Board BoardInGame { get => boardInGame; set => boardInGame = value; }
    public int TotalTurns { get => totalTurns; set => totalTurns = value; }
    public int StepsInTurn { get => stepsInTurn; set => stepsInTurn = value; }
    public int CardsPlayerdInTurn { get => cardsPlayerdInTurn; set => cardsPlayerdInTurn = value; }
}

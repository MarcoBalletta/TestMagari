using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class Board : MonoBehaviour
{
    private GameMode gm;
    private Grid grid;
    private Dictionary<Vector2Int, Tile> mapTiles = new Dictionary<Vector2Int, Tile>();
    private NavMeshSurface navMesh;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private List<Card> listOfPossibleCards = new List<Card>();
    [SerializeField] private List<Card> deckOfCards = new List<Card>();

    public List<Card> DeckOfCards { get => deckOfCards; }
    public Dictionary<Vector2Int, Tile> MapTiles { get => mapTiles; set => mapTiles = value; }

    public void Init(GameMode gameMode)
    {
        gm = gameMode;
        grid = GetComponent<Grid>();
        CreateGrid();
        navMesh = GetComponent<NavMeshSurface>();
        gm.preGame += PopulateDeck;
        gm.gameStart += GiveCardsToPlayers;
        gm.pickCard += PlayerDrawCard;
        gm.movePlayerToken += BakeArea;
    }

    private void CreateGrid()
    {
        Vector3 startPosition = new Vector3(gm.GameState.Columns * (grid.cellSize.x + grid.cellGap.x) / 2 , 0, gm.GameState.Rows * (grid.cellSize.z + grid.cellGap.z) / 2);
        float x= startPosition.x;
        float y = startPosition.z;
        for(int row = 0; row < gm.GameState.Rows; row++)
        {
            for(int column = gm.GameState.Columns - 1; column >= 0; column--)
            {
                var tile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                tile.transform.SetParent(this.transform);
                tile.transform.localScale = grid.cellSize;
                x -= 1 * (grid.cellSize.x + grid.cellGap.x);
                tile.Setup(gm, row, column, TileType.empty);
                mapTiles[new Vector2Int(row, column)] = tile;
            }
            x = startPosition.x;
            y -= 1 * (grid.cellSize.z + grid.cellGap.z);
        }
    }
    private void PopulateDeck()
    {
        deckOfCards.Clear();
        foreach(var card in listOfPossibleCards)
        {
            for(int i = 0; i < card.NumberInTheDeck; i++)
            {
                deckOfCards.Add(card);
            }
        }
    }

    private void GiveCardsToPlayers()
    {
        foreach(var player in gm.GameState.PlayersInGame)
        {
            player.PlayerCards = new Card[Constants.MAX_NUMBER_OF_CARDS + 1];
            for (int i = 0; i< Constants.MAX_NUMBER_OF_CARDS; i++)
            {
                var randomIndex = DrawCard();
                player.PlayerCards[i] = deckOfCards[randomIndex];
                deckOfCards.RemoveAt(randomIndex);
            }
        }
    }

    private int DrawCard()
    {
        var randomIndex = Random.Range(0, deckOfCards.Count);
        while (deckOfCards[randomIndex] == null)
        {
            randomIndex = Random.Range(0, deckOfCards.Count);
        }
        return randomIndex;
    }

    private void PlayerDrawCard()
    {
        var player = gm.GameState.PlayersInGame[gm.GameState.PlayerTurn];
        var firstIndexEmptyCard = 0;
        foreach(var card in player.PlayerCards)
        {
            if(card == null)
            {
                var deckIndex = DrawCard();
                player.PlayerCards[firstIndexEmptyCard] = deckOfCards[deckIndex];
                deckOfCards.RemoveAt(deckIndex);
                return;
            }
            firstIndexEmptyCard++;
        }
    }

    public void ChooseEndingTile(PlayerManager player)
    {
        var tileIndex = new Vector2Int(Random.Range(0, gm.GameState.Rows), Random.Range(0, gm.GameState.Columns));
        while(mapTiles[tileIndex].Data.Type != TileType.empty || Vector3.Distance(player.StartingTile.transform.position, mapTiles[tileIndex].transform.position) < Constants.MINIMUM_DISTANCE_FROM_STARTINGTILE)
        {
            tileIndex = new Vector2Int(Random.Range(0, gm.GameState.Rows), Random.Range(0, gm.GameState.Columns));
        }
        player.EndingTile = mapTiles[tileIndex];
        player.EndingTile.Data.Type = TileType.ending;
        return;
    }

    public void CheckIfTileCanBeSpawned(int row, int column, Card card)
    {
        if(PositionableTileConditions(row, column))
        {
            foreach(var direction in card.CorridorDirections)
            {
                if (direction.HasFlag(Directions.north))
                {
                    if (row - 1 >= 0 && mapTiles[new Vector2Int(row - 1, column)].Data.Type == TileType.corridor && !CheckDirection(row - 1, column, Directions.south)) return;
                }
                else
                {
                    if (row - 1 >= 0 && mapTiles[new Vector2Int(row - 1, column)].Data.Type == TileType.corridor && CheckDirection(row - 1, column, Directions.south)) return;
                }
                if (direction.HasFlag(Directions.south))
                {
                    if (row + 1 < gm.GameState.Rows && mapTiles[new Vector2Int(row + 1, column)].Data.Type == TileType.corridor && !CheckDirection(row + 1, column, Directions.north)) return;
                }
                else
                {
                    if (row + 1 < gm.GameState.Rows && mapTiles[new Vector2Int(row + 1, column)].Data.Type == TileType.corridor && CheckDirection(row + 1, column, Directions.north)) return;
                }
                if (direction.HasFlag(Directions.east))
                {
                    if (column + 1 < gm.GameState.Columns && mapTiles[new Vector2Int(row, column + 1)].Data.Type == TileType.corridor && !CheckDirection(row, column + 1, Directions.west)) return;
                }
                else
                {
                    if (column + 1 < gm.GameState.Columns && mapTiles[new Vector2Int(row, column + 1)].Data.Type == TileType.corridor && CheckDirection(row, column + 1, Directions.west)) return;
                }
                if (direction.HasFlag(Directions.west))
                {
                    if (column - 1 >= 0 && mapTiles[new Vector2Int(row, column - 1)].Data.Type == TileType.corridor && !CheckDirection(row, column - 1, Directions.east)) return;
                }
                else
                {
                    if (column - 1 >= 0 && mapTiles[new Vector2Int(row, column - 1)].Data.Type == TileType.corridor && CheckDirection(row, column - 1, Directions.east)) return;
                }
            }
            //can spawn tile
            SpawnTileCorridor(row, column, card);
            gm.StateManager.ChangeState(Constants.STATE_MOVEPLAYERTOKEN_ID, gm);
        }
    }

    public bool PositionableTileConditions(int row, int column)
    {
        if ((mapTiles[new Vector2Int(row, column)].Data.Type != TileType.starting && mapTiles[new Vector2Int(row, column)].Data.Type != TileType.ending) && mapTiles[new Vector2Int(row, column)].PlayerOnTile == null) return true;
        else return false;
    }

    private bool CheckDirection(int row, int column, Directions direction)
    {
        foreach(var dir in mapTiles[new Vector2Int(row, column)].Data.CardTile.CorridorDirections)
        {
            if (dir.HasFlag(direction)) return true;
        }
        return false;
    }

    private void SpawnTileCorridor(int row, int column, Card card)
    {
        var corridorTile = Instantiate(card.CardObjectPrefab, transform);
        corridorTile.Setup(gm, row, column, TileType.corridor);
        corridorTile.SetupCard(card);
        Vector3 tilePosition = mapTiles[new Vector2Int(row, column)].transform.position;
        Destroy(mapTiles[new Vector2Int(row, column)].gameObject);
        gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].DiscardCard(card);
        gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].CardSelected = null;
        //check switch tiles
        if (mapTiles[new Vector2Int(row, column)].Data.Type == TileType.corridor)
        {
            gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].SwapTilesDrawCard(mapTiles[new Vector2Int(row, column)].Data.CardTile);
        }
        mapTiles[new Vector2Int(row, column)] = corridorTile;
        corridorTile.transform.position = tilePosition;
    }

    public void BakeArea()
    {
        navMesh.BuildNavMesh();
    }

    public bool CheckTilesConnected(TileData tile1, TileData tile2)
    {
        var direction = GetMovementDirection(tile1, tile2);
        switch (direction)
        {
            case Directions.north:
                return CheckConnectedTilesGivenDirection(Directions.north, Directions.south, tile1, tile2);
            case Directions.south:
                return CheckConnectedTilesGivenDirection(Directions.south, Directions.north, tile1, tile2);
            case Directions.west:
                return CheckConnectedTilesGivenDirection(Directions.west, Directions.east, tile1, tile2);               
            case Directions.east:
                return CheckConnectedTilesGivenDirection(Directions.east, Directions.west, tile1, tile2);
            default:
                return true;
        }
    }

    public Directions GetMovementDirection(TileData tile1, TileData tile2)
    {
        if(tile1.Row == tile2.Row)
        {
            if (tile1.Column - tile2.Column == 1)
            {
                return Directions.south;
            }
            else return Directions.north;
        }else
        {
            if (tile1.Row - tile2.Row == 1)
            {
                return Directions.west;
            }
            else return Directions.east;
        }
    }

    private bool CheckConnectedTilesGivenDirection(Directions dir1, Directions dir2, TileData tile1, TileData tile2)
    {
        if (tile1?.CardTile)
        {
            foreach (var direction1FromList in tile1.CardTile.CorridorDirections)
            {
                if (direction1FromList.HasFlag(Directions.west) && direction1FromList.HasFlag(gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].ComingFromDirection))
                {
                    foreach (var direction2FromList in tile2.CardTile.CorridorDirections)
                    {
                        if (direction2FromList.HasFlag(Directions.east)) return true;
                    }
                    return false;
                }
            }
            return false;
        }
        else return true;
    }

    public void MoveToken(PlayerManager tokenToMove, TileData tileToMoveTo)
    {
        var movement = new MoveUnitCommand(tileToMoveTo.Row, tileToMoveTo.Column, tokenToMove, this);
        movement.Execute();
    }
}

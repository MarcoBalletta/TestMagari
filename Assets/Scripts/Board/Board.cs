using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private GameMode gm;
    private Grid grid;
    private Dictionary<Vector2Int, Tile> mapTiles = new Dictionary<Vector2Int, Tile>();
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
        gm.preGame += PopulateDeck;
        gm.gameStart += GiveCardsToPlayers;
        gm.pickCard += PlayerDrawCard;
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
        return;
    }

    public void CheckIfTileCanBeSpawned(int row, int column, Card card)
    {
        if(PositionableTileConditions(row, column))
        {
            foreach(var direction in card.CorridorDirections)
            {
                switch (direction)
                {
                    case Directions.north:
                        if (row -1 >= 0 && !CheckDirection(row -1, column, Directions.south)) return;
                        break;
                    case Directions.south:
                        if(row + 1 < gm.GameState.Rows && !CheckDirection(row + 1, column, Directions.north)) return;
                        break;
                    case Directions.east:
                        if (column + 1 < gm.GameState.Columns && !CheckDirection(row, column + 1, Directions.west)) return;
                        break;
                    case Directions.west:
                        if (column - 1 >= 0 && !CheckDirection(row, column + 1, Directions.east)) return;
                        break;
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
            if (dir == direction) return true;
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
}

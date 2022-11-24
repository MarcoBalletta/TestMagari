using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class Board : MonoBehaviour
{
    protected GameMode gm;
    protected Grid grid;
    protected Dictionary<Vector2Int, Tile> mapTiles = new Dictionary<Vector2Int, Tile>();
    protected NavMeshSurface navMesh;
    private AudioSource source;
    [SerializeField] protected Tile tilePrefab;
    [SerializeField] protected AudioClip spawnTileClip;
    [SerializeField] protected List<Card> listOfPossibleCards = new List<Card>();
    [SerializeField] protected List<Card> deckOfCards = new List<Card>();
    [SerializeField] protected int numberOfImpactsOnTileSpawnedSound = 4;

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
        gm.pickCard += BakeArea;
        gm.movePlayerToken += BakeArea;
        gm.tilePlaced += SpawnTileSound;
        source = GetComponent<AudioSource>();
    }

    protected void CreateGrid()
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
                tile.Setup(gm, row, column, TileType.empty, false);
                mapTiles[new Vector2Int(row, column)] = tile;
            }
            x = startPosition.x;
            y -= 1 * (grid.cellSize.z + grid.cellGap.z);
        }
    }
    protected void PopulateDeck()
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

    protected virtual void GiveCardsToPlayers()
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

    protected int DrawCard()
    {
        var randomIndex = Random.Range(0, deckOfCards.Count);
        while (deckOfCards[randomIndex] == null)
        {
            randomIndex = Random.Range(0, deckOfCards.Count);
        }
        return randomIndex;
    }

    protected void PlayerDrawCard()
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

    public virtual void ChooseEndingTile(PlayerManager player)
    {
        var tileIndex = new Vector2Int(Random.Range(0, gm.GameState.Rows), Random.Range(0, gm.GameState.Columns));
        var valueToMeasure = 0;
        if (gm.GameState.Rows > gm.GameState.Columns) valueToMeasure = gm.GameState.Columns;
        else valueToMeasure = gm.GameState.Rows;
        while (mapTiles[tileIndex].Data.Type != TileType.empty || Vector3.Distance(player.StartingTile.transform.position, mapTiles[tileIndex].transform.position) < valueToMeasure/2)
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
            if(CheckDirection(card, Directions.north))
            {
                if (row - 1 >= 0 && mapTiles[new Vector2Int(row - 1, column)].Data.Type == TileType.corridor && !CheckDirection(mapTiles[new Vector2Int(row - 1, column)].Data.CardTile, Directions.south)) return;
            }
            else
            {
                if (row - 1 >= 0 && mapTiles[new Vector2Int(row - 1, column)].Data.Type == TileType.corridor && CheckDirection(mapTiles[new Vector2Int(row - 1, column)].Data.CardTile, Directions.south))  return;
                if (mapTiles[new Vector2Int(row - 1, column)].Data.Type == TileType.starting || mapTiles[new Vector2Int(row - 1, column)].Data.Type == TileType.ending) return;
            }
            if(CheckDirection(card, Directions.south))
            {
                if (row + 1 < gm.GameState.Rows && mapTiles[new Vector2Int(row + 1, column)].Data.Type == TileType.corridor && !CheckDirection(mapTiles[new Vector2Int(row + 1, column)].Data.CardTile, Directions.north))  return;
            }
            else
            {
                if (row + 1 < gm.GameState.Rows && mapTiles[new Vector2Int(row + 1, column)].Data.Type == TileType.corridor && !CheckDirection(mapTiles[new Vector2Int(row + 1, column)].Data.CardTile, Directions.north))  return;
                if (mapTiles[new Vector2Int(row + 1, column)].Data.Type == TileType.starting || mapTiles[new Vector2Int(row + 1, column)].Data.Type == TileType.ending) return;
            }
            if (CheckDirection(card, Directions.east))
            {
                if (column + 1 < gm.GameState.Columns && mapTiles[new Vector2Int(row, column + 1)].Data.Type == TileType.corridor && !CheckDirection(mapTiles[new Vector2Int(row, column + 1)].Data.CardTile, Directions.west)) return;
            }
            else
            {
                if (column + 1 < gm.GameState.Columns && mapTiles[new Vector2Int(row, column + 1)].Data.Type == TileType.corridor && CheckDirection(mapTiles[new Vector2Int(row, column + 1)].Data.CardTile, Directions.west))  return;
                if (mapTiles[new Vector2Int(row, column + 1)].Data.Type == TileType.starting || mapTiles[new Vector2Int(row, column + 1)].Data.Type == TileType.ending) return;
            }
            if(CheckDirection(card, Directions.west))
            {
                if (column - 1 >= 0 && mapTiles[new Vector2Int(row, column - 1)].Data.Type == TileType.corridor && !CheckDirection(mapTiles[new Vector2Int(row, column - 1)].Data.CardTile, Directions.east))   return;
            }
            else
            {
                if (column - 1 >= 0 && mapTiles[new Vector2Int(row, column - 1)].Data.Type == TileType.corridor && CheckDirection(mapTiles[new Vector2Int(row, column - 1)].Data.CardTile, Directions.east))    return;
                if (mapTiles[new Vector2Int(row, column - 1)].Data.Type == TileType.starting || mapTiles[new Vector2Int(row, column - 1)].Data.Type == TileType.ending) return;
            }
            //can spawn tile
            SpawnTileCorridor(row, column, card);
            gm.IncreaseCardsPlayed();
            if(!gm.CanPlayCardAgain())   gm.StateManager.ChangeState(Constants.STATE_MOVEPLAYERTOKEN_ID, gm);
        }
    }

    public bool PositionableTileConditions(int row, int column)
    {
        if ((mapTiles[new Vector2Int(row, column)].Data.Type != TileType.starting && mapTiles[new Vector2Int(row, column)].Data.Type != TileType.ending) && mapTiles[new Vector2Int(row, column)].PlayerOnTile == null) return true;
        else return false;
    }

    protected bool CheckDirection(Card cardTile, Directions direction)
    {
        foreach(var dir in cardTile.CorridorDirections)
        {
            if (dir.Direction.HasFlag(direction)) return true;
        }
        return false;
    }

    protected virtual void SpawnTileCorridor(int row, int column, Card card)
    {
        var corridorTile = Instantiate(card.CardObjectPrefab, transform);
        corridorTile.Setup(gm, row, column, TileType.corridor, DecideIfTrap());
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
        if(gm.tilePlaced != null) gm.tilePlaced();

        //gm.enableTableCamera();
    }

    protected void SpawnTileSound()
    {
        StartCoroutine(SoundRepeated());
    }

    protected IEnumerator SoundRepeated()
    {
        for(int i = 0; i < numberOfImpactsOnTileSpawnedSound; i++)
        {
            source.PlayOneShot(spawnTileClip);
            yield return new WaitForSeconds(0.4f);
        }
    }

    private bool DecideIfTrap()
    {
        var percentage = Random.Range(0, 100);
        if (percentage < Constants.PERCENTAGE_IF_TRAP)  return true;
        else return false;
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
                return Directions.west;
            }
            else return Directions.east;
        }else
        {
            if (tile1.Row - tile2.Row == 1)
            {
                return Directions.north;
            }
            else return Directions.south;
        }
    }

    protected bool CheckConnectedTilesGivenDirection(Directions dir1, Directions dir2, TileData tile1, TileData tile2)
    {
        if (tile1?.CardTile)
        {
            foreach (var direction1FromList in tile1.CardTile.CorridorDirections)
            {
                if (direction1FromList.Direction.HasFlag(dir1) && direction1FromList.Direction.HasFlag(gm.GameState.PlayersInGame[gm.GameState.PlayerTurn].ComingFromDirection))
                {
                    if (tile2?.CardTile)
                    {
                        foreach (var direction2FromList in tile2.CardTile.CorridorDirections)
                        {
                            if (direction2FromList.Direction.HasFlag(dir2)) return true;
                        }
                        return false;
                    }
                    else return true;
                }
            }
            return false;
        }
        else return true;
    }

    public void MoveToken(PlayerManager tokenToMove, TileData tileToMoveTo)
    {
        var direction = GetMovementDirection(tokenToMove.ActualTile.Data, tileToMoveTo);
        var movement = new MoveUnitCommand(tileToMoveTo.Row, tileToMoveTo.Column, tokenToMove, this, direction);
        movement.Execute();
    }
}

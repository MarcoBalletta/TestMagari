using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class TutorialBoard : Board
{

    protected override void GiveCardsToPlayers()
    {
        foreach (var player in gm.GameState.PlayersInGame)
        {
            player.PlayerCards = new Card[1];
            for (int i = 0; i < 1; i++)
            {
                var randomIndex = DrawCard();
                player.PlayerCards[i] = deckOfCards[randomIndex];
                deckOfCards.RemoveAt(randomIndex);
            }
        }
    }

    public override void ChooseEndingTile(PlayerManager player)
    {
        var tileIndex = new Vector2Int(0,2);
        player.EndingTile = mapTiles[tileIndex];
        player.EndingTile.Data.Type = TileType.ending;
        return;
    }

    protected override void SpawnTileCorridor(int row, int column, Card card)
    {
        var corridorTile = Instantiate(card.CardObjectPrefab, transform);
        corridorTile.Setup(gm, row, column, TileType.corridor, false);
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
        if (gm.tilePlaced != null) gm.tilePlaced();
        BakeArea();
        //gm.enableTableCamera();
    }
}

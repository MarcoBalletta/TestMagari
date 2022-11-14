using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardHandler : MonoBehaviour, IPointerClickHandler
{
    private Card card;
    private GameMode gameMode;

    public void Setup(Card cardData, GameMode gm)
    {
        card = cardData;
        gameMode = gm;
        GetComponent<Image>().sprite = card.Image;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(gameMode.StateManager.Current.Name == Constants.STATE_PICKCARD_ID)
        {
            gameMode.GameState.PlayersInGame[gameMode.GameState.PlayerTurn].CardSelected = card;
            gameMode.pickedCard();
        }else if(gameMode.StateManager.Current.Name == Constants.INFO_DISCARD_CARD)
        {
            gameMode.GameState.PlayersInGame[gameMode.GameState.PlayerTurn].DiscardCard(card);
        }
    }
}
